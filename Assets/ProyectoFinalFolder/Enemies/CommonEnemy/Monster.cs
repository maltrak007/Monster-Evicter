using System;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Components;
using ProyectoFinalFolder.Enemies.EnemyComponents;
using ProyectoFinalFolder.Enemies.State;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.CommonEnemy
{
    [RequireComponent(typeof(EnemyHealthComponent)),RequireComponent(typeof(AffinityComponent)),RequireComponent(typeof(MonsterStateMachine)),RequireComponent(typeof(MonsterSoundRelay))]
    public class Monster : MonoBehaviour
    {
        private EnemyHealthComponent monsterHealthComponent;
        private AffinityComponent affinityComponent;
        private MonsterStateMachine monsterStateMachine;
        public MonsterSoundRelay monsterSoundRelay;
        public EnemyInteractableComponent monsterInteractableComponent;
        public Transform popUpPosition;
        private bool hasBeenDamaged = false;
        public bool isStunned { get; private set; } = false;
        
        [Header("Respawn Settings")]
        public string monsterID;
        
        [Header("Mana Drop Settings")]
        [SerializeField] private GameObject manaOrbPrefab;
        [SerializeField] private int numberOfOrbs;
        [SerializeField] private float launchForce = 5f;
        [SerializeField] private float arcHeight = 2f;
        [SerializeField] private DamagePopUpGenerator popUpGenerator;
        
        private void Awake()
        {
            monsterHealthComponent = GetComponent<EnemyHealthComponent>();
            affinityComponent = GetComponent<AffinityComponent>();
            monsterStateMachine = GetComponent<MonsterStateMachine>();
            monsterSoundRelay = GetComponent<MonsterSoundRelay>();
        }
        
        private void Start()
        {
            monsterHealthComponent.onHealthChanged.AddListener(OnEnemyHitReact);
            monsterHealthComponent.onDeath.AddListener(OnEnemyStun);
            monsterHealthComponent.onEnemyRevive.AddListener(OnEnemyRevive);
            monsterInteractableComponent.onInteract.AddListener(OnEnemyEviction);
            numberOfOrbs = GameManagerScript.Instance.playerUpgrades.TryGetBestIntUpgrade("manaOrbsSpawn", out int orbValue) ? orbValue : 1;
            monsterID = monsterStateMachine.monsterID;
        }
        
        private void OnDestroy()
        {
            monsterHealthComponent.onHealthChanged.RemoveListener(OnEnemyHitReact);
            monsterHealthComponent.onDeath.RemoveListener(OnEnemyStun);
            monsterHealthComponent.onEnemyRevive.RemoveListener(OnEnemyRevive);
            monsterInteractableComponent.onInteract.RemoveListener(OnEnemyEviction);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("PlayerProjectile"))
            {
                if (!hasBeenDamaged)
                {
                    OnEnemyDamage();
                }
                if(isStunned) return;
                Projectile projectile = other.gameObject.GetComponent<Projectile>();
                if(projectile.GetAffinity() == affinityComponent.GetAffinityWeakness())
                {
                    monsterHealthComponent.TakeDamage(other.gameObject.GetComponent<Projectile>().GetDamage() * other.gameObject.GetComponent<Projectile>().GetWeaknessMultiplicator());
                    DamagePopUpGenerator.DamageType damageTypeWeakness = DamagePopUpGenerator.DamageType.Weakness;
                    popUpGenerator.GeneratePopUp(other.gameObject.GetComponent<Projectile>().GetDamage() * other.gameObject.GetComponent<Projectile>().GetWeaknessMultiplicator(), damageTypeWeakness, projectile.GetAffinity(), popUpPosition.position);
                    ManaDropSpawner.SpawnManaDrops(transform.position + Vector3.up * 1f, manaOrbPrefab, numberOfOrbs, launchForce, arcHeight);
                }
                else if (projectile.GetAffinity() == affinityComponent.GetAffinityResistance())
                {
                    monsterHealthComponent.TakeDamage(other.gameObject.GetComponent<Projectile>().GetDamage() / 2);
                    DamagePopUpGenerator.DamageType damageTypeResistance = DamagePopUpGenerator.DamageType.Resistance;
                    popUpGenerator.GeneratePopUp(other.gameObject.GetComponent<Projectile>().GetDamage()/ 2, damageTypeResistance, projectile.GetAffinity(),popUpPosition.position);
                }
                else
                {
                    monsterHealthComponent.TakeDamage(other.gameObject.GetComponent<Projectile>().GetDamage());
                    DamagePopUpGenerator.DamageType damageTypeNormal = DamagePopUpGenerator.DamageType.Normal;
                    popUpGenerator.GeneratePopUp(other.gameObject.GetComponent<Projectile>().GetDamage(), damageTypeNormal, projectile.GetAffinity(),popUpPosition.position);
                }
            }
        }

        private void OnEnemyHitReact()
        {
            monsterStateMachine.SwitchState(new EnemyHitReactState(monsterStateMachine));
        }

        private void OnEnemyStun()
        {
            isStunned = true;
            monsterStateMachine.SwitchState(new EnemyStunnedState(monsterStateMachine));
        }

        private void OnEnemyRevive()
        {
            isStunned = false;
            monsterStateMachine.SwitchState(new EnemyCombatState(monsterStateMachine));
        }

        private void OnEnemyEviction()
        {
            monsterStateMachine.SwitchState(new EnemyDeathState(monsterStateMachine));
        }

        private void OnEnemyDamage()
        {
            hasBeenDamaged = true;
            monsterStateMachine.SwitchState(new EnemyCombatState(monsterStateMachine));
        }
    }
}