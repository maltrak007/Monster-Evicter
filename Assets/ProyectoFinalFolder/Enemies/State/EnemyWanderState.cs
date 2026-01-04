using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;
using UnityEngine.AI;

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyWanderState : MonsterBaseState
    {
        public EnemyWanderState(MonsterStateMachine stateMachine) : base(stateMachine) {}
        
        private readonly int locomotionHash = Animator.StringToHash("Locomotion");
        
        private readonly int speedHash = Animator.StringToHash("Speed");
        
        private const float CrossFadeDuration = 0.1f;
        
        private const float AnimatorDampTime = 0.1f;
        
        private const float WanderRadius = 5f;
        
        public override void Enter()
        {
            stateMachine.animator.CrossFadeInFixedTime(locomotionHash, CrossFadeDuration);
            stateMachine.animator.SetFloat(speedHash, 1f);
            Vector3 point;
            if (CalculateWanderPosition(stateMachine.transform.position, WanderRadius, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                stateMachine.monsterAgent.SetDestination(point);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (IsInChaseRange())
            {
                // Transition to chase state if the player is within detection range
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }
            if (stateMachine.monsterAgent.remainingDistance <= stateMachine.monsterAgent.stoppingDistance)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            stateMachine.animator.CrossFadeInFixedTime(locomotionHash, CrossFadeDuration);
            stateMachine.animator.SetFloat(speedHash, 1f);
        }

        public override void Exit()
        {
            
        }
        
        private bool CalculateWanderPosition(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }
    }
}