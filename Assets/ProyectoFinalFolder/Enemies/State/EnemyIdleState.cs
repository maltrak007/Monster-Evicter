using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyIdleState : MonsterBaseState
    {
        public EnemyIdleState(MonsterStateMachine stateMachine) : base(stateMachine) {}
        
        private readonly int locomotionHash = Animator.StringToHash("Locomotion");
        
        private readonly int speedHash = Animator.StringToHash("Speed");
        
        private const float CrossFadeDuration = 0.1f;
        
        private const float AnimatorDampTime = 0.1f;
        
        private float idleDuration = 2f; // Duration to stay idle before wandering again
        
        public override void Enter()
        {
            stateMachine.animator.CrossFadeInFixedTime(locomotionHash, CrossFadeDuration);
            stateMachine.animator.SetFloat(speedHash, 0f);
            idleDuration = 2f; // Reset idle duration
        }
        
        public override void Tick(float deltaTime)
        {
            if (IsInChaseRange())
            {
                // Transition to chase state if the player is within detection range
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }
            idleDuration -= deltaTime;
            if (idleDuration <= 0f)
            {
                // Transition to wander state after being idle for a certain duration
                stateMachine.SwitchState(new EnemyWanderState(stateMachine));
                return;
            }
            stateMachine.animator.SetFloat(speedHash, 0f, AnimatorDampTime, deltaTime);
        }
        
        public override void Exit()
        {
            
        }
    }
}