using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyHitReactState : MonsterBaseState
    {
        public EnemyHitReactState(MonsterStateMachine stateMachine) : base(stateMachine) {}

        private readonly int HitReactHash = Animator.StringToHash("HitReact");
        
        private const float CrossFadeDuration = 0.1f;

        private float cooldownHitReactTimer = 1f;
        
        public override void Enter()
        {
            stateMachine.animator.CrossFadeInFixedTime(HitReactHash, CrossFadeDuration);
            SoundManagerScript.Instance.PlaySound("SlimeHit");
        }
    
        public override void Tick(float deltaTime)
        {
            cooldownHitReactTimer -= deltaTime;
            if (cooldownHitReactTimer <= 0f)
            {
                stateMachine.SwitchState(new EnemyCombatState(stateMachine));
            }
        }

        public override void Exit()
        {
            
        }
    }
}