using ProyectoFinalFolder.Enemies.CommonEnemy;
using ProyectoFinalFolder.Enemies.EnemyComponents;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyStunnedState : MonsterBaseState
    {
        public EnemyStunnedState(MonsterStateMachine stateMachine) : base(stateMachine) {}
        
        private readonly int StunnedHash = Animator.StringToHash("Stunned");
        
        private const float CrossFadeDuration = 0.1f;
        
        private float reviveTimer = 6f;
        
        public override void Enter()
        {
            stateMachine.animator.CrossFadeInFixedTime(StunnedHash, CrossFadeDuration);
            stateMachine.monsterAgent.ResetPath();
            stateMachine.monsterAgent.velocity = Vector3.zero;
            reviveTimer = 6f;
            stateMachine.gameObject.GetComponent<Monster>().monsterInteractableComponent.SetCanInteract(true);
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.GetComponent<Monster>().monsterInteractableComponent.hasPlayerEvicted)
            {
                stateMachine.SwitchState(new EnemyDeathState(stateMachine));
                return;
            }
            reviveTimer -= deltaTime;
            if (!(reviveTimer <= 0f)) return;
            float hpToRegenerate = stateMachine.GetComponent<EnemyHealthComponent>().GetMaxHealth() * 0.4f;
            stateMachine.GetComponent<EnemyHealthComponent>().Heal(hpToRegenerate);
            stateMachine.GetComponent<EnemyHealthComponent>().EnemyRevive();
            stateMachine.SwitchState(new EnemyCombatState(stateMachine));
        }

        public override void Exit()
        {
            stateMachine.gameObject.GetComponent<Monster>().monsterInteractableComponent.SetCanInteract(false);
        }
    }
}