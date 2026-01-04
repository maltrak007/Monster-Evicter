using System.Collections;
using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;


//I HAVE TO IMPROVE THIS CLASS SO THE ENEMIES CAN PERFORM MULTIPLE ATTACKS BASED ON AN ARRAY OF AVAILABLE MOVES AND CONDITIONS

namespace ProyectoFinalFolder.Enemies.State
{
    public class EnemyCombatState : MonsterBaseState
    {
        public EnemyCombatState(MonsterStateMachine stateMachine) : base(stateMachine) { }
        
        private readonly int AttackHash = Animator.StringToHash("Attack_1");
        
        private const float TransitionDuration = 0.1f;
        
        bool CanAttack = true;
        
        private Coroutine cooldownAttackCoroutine;
        
        private float attackCooldown = 1.5f;
        
        public override void Enter()
        {
            
        }

        public override void Tick(float deltaTime)
        {
            float distanceToPlayer = Vector3.Distance(
                stateMachine.transform.position,
                stateMachine.playerReference.transform.position
            );

            if (distanceToPlayer > stateMachine.playerAttackRange && !stateMachine.isAttacking)
            {
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }

            // Face the player
            FacePlayer();

            // Attempt to attack
            if (CanAttack)
            {
                PerformAttack();
            }
        }

        public override void Exit()
        {
            if (cooldownAttackCoroutine != null)
            {
                stateMachine.StopCoroutine(cooldownAttackCoroutine);
                cooldownAttackCoroutine = null;
            }
            //Double check if the animation is interrupted
            stateMachine.DeactivateMonsterCollider();
            
            stateMachine.monsterAgent.ResetPath();
            stateMachine.monsterAgent.velocity = Vector3.zero;
        }
        
        public IEnumerator CooldownAttack()
        {
            CanAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            CanAttack = true;
        }
        
        private void PerformAttack()
        {
            CanAttack = false;

            // Play the attack animation
            stateMachine.animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);

            // Start the cooldown
            cooldownAttackCoroutine = stateMachine.StartCoroutine(CooldownAttack());
        }
    }
}