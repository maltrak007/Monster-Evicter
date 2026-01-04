using ProyectoFinalFolder.Common.States;
using ProyectoFinalFolder.Enemies.State;
using UnityEngine;
using UnityEngine.AI;

namespace ProyectoFinalFolder.Enemies.CommonEnemy
{
    public class MonsterStateMachine : StateMachine
    {
        [field:SerializeField] public Animator animator { get; private set; }
    
        [field:SerializeField] public float playerDetectionRange { get;  private set; }
    
        [field:SerializeField] public float playerAttackRange { get;  private set; }
    
        [field:SerializeField] public GameObject playerReference { get; private set; }
    
        [field:SerializeField] public NavMeshAgent monsterAgent { get; private set; }
    
        [field:SerializeField] public Collider monsterCollider { get; private set; }
        
        [field:SerializeField] public int monsterBounty { get; private set; }
        
        [field:SerializeField] public string monsterID { get; private set; }
        
        [field:SerializeField] public bool monsterBoss { get; private set; }
        [field:SerializeField] public bool isAttacking { get; private set; }
        
        private void Start()
        {
            playerReference = GameObject.FindGameObjectWithTag("Player");
            monsterAgent = GetComponent<NavMeshAgent>();
            if (monsterCollider != null)
            {
                monsterCollider.gameObject.SetActive(false);
            }
            // Initialize the state machine
            SwitchState(new EnemyIdleState(this));
        }

        private void Update()
        {
            TickCurrentState(Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw a sphere in the editor to visualize the player detection range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        }
    
        public void ActivateMonsterCollider()
        {
            monsterCollider.gameObject.SetActive(true);
        }
        
        public void DeactivateMonsterCollider()
        {
            monsterCollider.gameObject.SetActive(false);
        }
        
        public void SetTrueIsAttacking()
        {
            isAttacking = true;
        }
        
        public void SetFalseIsAttacking()
        {
            isAttacking = false;
        }
    }
}
