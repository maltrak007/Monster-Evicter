using System.Collections.Generic;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Components.PoolSystem;
using ProyectoFinalFolder.Player;
using ProyectoFinalFolder.Player.Scripts;
using ProyectoFinalFolder.Player.Scripts.MiscScripts;
using ProyectoFinalFolder.Player.Scripts.PlayerController;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerManaComponent)),DefaultExecutionOrder(-100)]
public class PlayerSpells : MonoBehaviour
{
    [Header("Attack Settings")] [SerializeField]
    private Projectile[] projectilePrefabs;
    [SerializeField] private Camera characterCamera;

    [Header("Elemental Affinities")] private int currentAffinityIndex = 0;
    public Affinities currentAffinity;
    public List<Affinities> unlockedAffinities = new List<Affinities>();
    private Dictionary<Affinities, Projectile> affinityToProjectile;
    public UnityEvent<Affinities> onAffinityChanged = new UnityEvent<Affinities>();

    [Header("Projectile Spell Settings")] [SerializeField]
    private Transform launchPosition;
    
    //TODO:SUBSTITUE VALUES FOR THE PLAYER UPGRADE VALUES
    public float elementalBallCost = 8f;

    [Header("Shield Spell Settings")] public GameObject magicShieldPrefab;
    public float elementalShieldCost = 0.2f;

    [Header("Area Attack Settings")] 
    //TODO:SUBSTITUE VALUES FOR THE PLAYER UPGRADE VALUES
    public float elementalAreaCost = 20f;
    
    [Header("Heal Settings")]
    private PlayerHealthPotion playerHealthPotion;
    
    private PlayerManaComponent manaComponent;
    private PlayerLocomotionInput playerInput;
    private Animator characterAnimator;
    private PlayerHealthComponent healthComponent;
    private PlayerUpgrades playerUpgrades;
    private static readonly int EBallSpeed = Animator.StringToHash("EBallSpeed");

    private void Awake()
    {
        manaComponent = GetComponent<PlayerManaComponent>();
        characterAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerLocomotionInput>();
        healthComponent = GetComponent<PlayerHealthComponent>();
        playerHealthPotion = GetComponent<PlayerHealthPotion>();
        playerUpgrades = GetComponent<PlayerUpgrades>();
    }

    private void Start()
    {
        unlockedAffinities.Add(Affinities.Fire);
        unlockedAffinities.Add(Affinities.Water);
        unlockedAffinities.Add(Affinities.Electric);
        
        currentAffinity = unlockedAffinities[0];
        currentAffinityIndex = unlockedAffinities.IndexOf(currentAffinity);
        
        affinityToProjectile = new Dictionary<Affinities, Projectile>();
        
        manaComponent.ChangeColourByAffinity(currentAffinity);

        playerInput.onElementalBallAttackEvent.AddListener(ElementBallAttack);
        playerInput.onElementalShieldEvent.AddListener(ActivateShield);
        playerInput.onElementalShieldReleaseEvent.AddListener(DeactivateShield);
        playerInput.onElementalAreaAttackEvent.AddListener(ElementalAreaAttack);
        playerInput.onHealthAction.AddListener(HealItself);
        playerInput.onChangeElement.AddListener(ChangeElement);
    }

    private void OnDestroy()
    {
        playerInput.onElementalBallAttackEvent.RemoveListener(ElementBallAttack);
        playerInput.onElementalShieldEvent.RemoveListener(ActivateShield);
        playerInput.onElementalShieldReleaseEvent.RemoveListener(DeactivateShield);
        playerInput.onElementalAreaAttackEvent.RemoveListener(ElementalAreaAttack);
        playerInput.onHealthAction.RemoveListener(HealItself);
        playerInput.onChangeElement.RemoveListener(ChangeElement);
    }
    
    private void ElementBallAttack()
    {
        if (!(manaComponent.currentMana >= elementalBallCost)) return;
        characterAnimator.SetBool("canEBall", true);
    }

    public void ActivateShield()
    {
        if (manaComponent.currentMana >= elementalShieldCost)
        {
            magicShieldPrefab.SetActive(true);
            SoundManagerScript.Instance.PlaySound("ShieldUp");
        }
    }

    public void DeactivateShield()
    {
        magicShieldPrefab.SetActive(false);
        SoundManagerScript.Instance.PlaySound("ShieldDown");
    }

    private void ElementalAreaAttack()
    {
        if (!(manaComponent.currentMana >= elementalBallCost)) return;
        characterAnimator.SetBool("canEArea", true);
    }

    public void HealItself()
    {
        playerHealthPotion.RestoreHealth(healthComponent);
    }

    private void CalculateProjectileDirection()
    {
        LayerMask layerToIgnore = LayerMask.GetMask("Shield", "DetectionCollider", "Player", "Projectile");
        Vector3 launchDirection;
        if (Physics.Raycast(characterCamera.transform.position, characterCamera.transform.forward, out RaycastHit hit,
                100f, ~layerToIgnore))
        {
            launchDirection = (hit.point - launchPosition.position).normalized;
        }
        else
        {
            launchDirection = characterCamera.transform.forward;
        }

        if (!affinityToProjectile.TryGetValue(currentAffinity, out Projectile prefab))
        {
            return;
        }

        GameObject projectileObj = ObjectPoolManager.SpawnGameObject(prefab.gameObject, launchPosition.position,
            Quaternion.LookRotation(launchDirection), ObjectPoolManager.PoolType.Projectiles);

        if (projectileObj != null)
        {
            Projectile instance = projectileObj.GetComponent<Projectile>();
            instance.Launch(launchDirection);
        }
    }
    
    private void ChangeElement()
    {
        if (unlockedAffinities == null || unlockedAffinities.Count == 0)
            return;

        currentAffinityIndex = (currentAffinityIndex + 1) % unlockedAffinities.Count;
        currentAffinity = unlockedAffinities[currentAffinityIndex];
        onAffinityChanged.Invoke(currentAffinity);
    }
    
    public void UpdatePlayerSpellParameters()
    {
        float projectileDamage = playerUpgrades.TryGetBestFloatUpgrade("eBallDamage",out var damage) ? damage : 5f;
        int projectileWeakMultiplier = playerUpgrades.TryGetBestIntUpgrade("eBallWeaknessCrit", out var weakMultiplier) ? weakMultiplier : 2;
        characterAnimator.SetFloat(EBallSpeed, playerUpgrades.TryGetBestFloatUpgrade("eBallAttackSpeedMultiplier", out var attackSpeed) ? attackSpeed : 1f);
        elementalShieldCost = playerUpgrades.TryGetBestFloatUpgrade("eShieldConsumption", out var shieldCost) ? shieldCost : 0.2f;
        foreach (var projectile in projectilePrefabs)
        {
            var affinity = projectile.GetComponent<Projectile>().GetAffinity();
            
            projectile.SetDamage(projectileDamage);
            
            projectile.SetWeaknessMultiplicator(projectileWeakMultiplier);
            affinityToProjectile.TryAdd(affinity, projectile);
        }
    }
}