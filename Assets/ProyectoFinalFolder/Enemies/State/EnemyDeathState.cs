using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Common.Manager.LevelManager;
using ProyectoFinalFolder.Common.Manager.SaveSystem;
using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyDeathState : MonsterBaseState
    {
        public EnemyDeathState(MonsterStateMachine stateMachine) : base(stateMachine) {}
        
        private readonly int DeathHash = Animator.StringToHash("Death");
        
        private const float CrossFadeDuration = 0.1f;
        
        private float DeathDuration = 5f;
        public override void Enter()
        {
            stateMachine.animator.CrossFadeInFixedTime(DeathHash, CrossFadeDuration);
            DisableComponents();
            GameManagerScript.Instance.playerInventory.AddCoinAmount(stateMachine.monsterBounty);
            if (stateMachine.monsterBoss)
            {
                BossLevelManager.Instance.MarkBossAsDefeated(stateMachine.monsterID);
            }
            else
            {
                MonsterLevelManager.Instance.MarkMonsterAsDefeated(stateMachine.monsterID);
            }
            SaveSystem.Save();
        }

        public override void Tick(float deltaTime)
        {
            // Disable the NavMeshAgent and any other components that should not be active during death
            DeathDuration -= deltaTime;
            if(DeathDuration <= 0f)
            {
                SoundManagerScript.Instance.PlaySound("EvictSound");
                stateMachine.gameObject.transform.position = new Vector3(1000f, 1000f, 1000f); // Move out of the scene
                stateMachine.gameObject.SetActive(false);
            }
        }

        public override void Exit()
        {
            
        }

        private void DisableComponents()
        {
            // Disable any components that should not be active during death
            stateMachine.monsterAgent.enabled = false;
            //stateMachine.monsterAgent.ResetPath();
            stateMachine.monsterAgent.velocity = Vector3.zero;
            stateMachine.DeactivateMonsterCollider();
            //stateMachine.monsterAgent.isStopped = true;
            stateMachine.monsterAgent.velocity = Vector3.zero;
            stateMachine.gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}