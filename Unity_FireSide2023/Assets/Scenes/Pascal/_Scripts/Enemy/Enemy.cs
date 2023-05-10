using UnityEngine;


namespace Pascal {



    public class Enemy : MonoBehaviour
    {
        [SerializeField] float attackRange = 1f;
        [SerializeField] int damage = 10;
        [Tooltip("How far can it travel away from initial position")]
        [SerializeField] float leashLenght = 300f; 
        
        [SerializeField] AudioManager audioManager;
        [SerializeField] EnemyMovementController movementController;

        public enum State { Idle, Chasing, Returning }
        public State state = State.Idle;

        Transform target;
        EnemyAggro aggro;
        Vector3 posInit;

        bool attackReady = true;
        float attackCooldown = 2f;


        void Start() {
            posInit = transform.position;
            aggro = GetComponent<EnemyAggro>();


            aggro.a_Trigger += HandleStartAggro;
            movementController.a_Stop += HandleStopChase;
            CharacterHealth.A_Death += HandleDeath;
        }


        void Update() {
            switch (state) {

                case State.Returning:
                    UpdateReturningState();
                    break;

                case State.Chasing:
                    UpdateChasingState();
                    break;

                case State.Idle:
                    UpdateIdleState();
                    break;
                
                default: break;
            }
        }



        void UpdateReturningState() {
            if (Vector3.Distance(transform.position, posInit) < 0.5f) {
                state = State.Idle;
                aggro.hasTarget = false;
            }
        }

        void UpdateChasingState() {
            if (Vector3.Distance(transform.position, target.position) < attackRange) {
                Attack();
            }
            if (Vector3.Distance(transform.position, posInit) > leashLenght) {
                movementController.StopChase();
            }
        }

        void UpdateIdleState() {
            // do nothing
        }





        void Attack() {
            if (!attackReady) return;

            
            
            // Deal damage to the player
            CharacterHealth CharacterHealth = target.GetComponent<CharacterHealth>();

            if (CharacterHealth != null) {
                CharacterHealth.TakeDamage(damage);
                audioManager.Play("Attack");
                attackReady = false;
            }

            // movementController.Pause(attackCooldown);
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        void ResetAttack() => attackReady = true;




        void HandleStartAggro(Transform target) {
            this.target = target;
            movementController.ChastTarget(target);
            state = State.Chasing;
            audioManager.Play("Aggro");
        }

        void HandleStopChase() {
            this.target = null;
            state = State.Returning;
            movementController.MoveToPoint(posInit);
        }

        void HandleDeath(string charName) {
            if (charName != this.name) return;
            Debug.Log($"{this.name} died");
        }

        void OnDrawGizmosSelected() {
            // Draw attack radius gizmo in editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }


}
