using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyChaseState : MonsterBaseState
    {
        public EnemyChaseState(MonsterStateMachine stateMachine) : base(stateMachine) {}

        private readonly int locomotionHash = Animator.StringToHash("Locomotion");

        private readonly int speedHash = Animator.StringToHash("Speed");

        private const float CrossFadeDuration = 0.1f;

        private const float AnimatorDampTime = 0.1f;

        public override void Enter()
        {
            stateMachine.animator.CrossFadeInFixedTime(locomotionHash, CrossFadeDuration);
            stateMachine.animator.SetFloat(speedHash, 1f);
        }

        public override void Tick(float deltaTime)
        {
            // if (!IsInChaseRange())
            // {
            //     // Transition to idle state if the player is out of detection range
            //     stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            //     return;
            // }
            // else
            if (IsInAttackRange())
            {
                stateMachine.SwitchState(new EnemyCombatState(stateMachine));
                return;
            }
            
            FacePlayer();
            MoveToPlayer(Time.deltaTime);
            
            stateMachine.animator.SetFloat(speedHash, 1f, AnimatorDampTime, deltaTime);
        }
        
        public override void Exit()
        {
            stateMachine.monsterAgent.ResetPath();
            stateMachine.monsterAgent.velocity = Vector3.zero;
        }

        private void MoveToPlayer(float deltaTime)
        {
            stateMachine.monsterAgent.SetDestination(GetDirectionToPlayer());
            stateMachine.monsterAgent.Move(GetDirectionToPlayer() * (stateMachine.monsterAgent.speed * deltaTime));
        }
        
        protected bool IsInAttackRange()
        {
            float distanceToPlayer = (stateMachine.playerReference.transform.position - stateMachine.transform.position).sqrMagnitude;
            return distanceToPlayer <= stateMachine.playerAttackRange * stateMachine.playerAttackRange;
        }
    }
}