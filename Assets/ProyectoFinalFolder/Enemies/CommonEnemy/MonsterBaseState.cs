using UnityEngine;

namespace ProyectoFinalFolder.Enemies.CommonEnemy
{
    public abstract class MonsterBaseState : Common.States.State
    {
        [field:SerializeField] protected MonsterStateMachine stateMachine;

        public MonsterBaseState(MonsterStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    
        protected bool IsInChaseRange()
        {
            float distanceToPlayer = (stateMachine.playerReference.transform.position - stateMachine.transform.position).sqrMagnitude;
            return distanceToPlayer <= stateMachine.playerDetectionRange * stateMachine.playerDetectionRange;
        }
    
        protected Vector3 GetDirectionToPlayer()
        {
            return (stateMachine.playerReference.transform.position - stateMachine.transform.position).normalized;
        }
    
        protected void FacePlayer()
        {
            Vector3 directionToPlayer = GetDirectionToPlayer();
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}