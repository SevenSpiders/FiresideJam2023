using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {
    

    public class  EnemyMovementController : MonoBehaviour {

        public System.Action a_Stop;
        public System.Action<int, Vector3> a_TargetReached;   // int: id, Vector3: target position


        public int id;

        [SerializeField] Rigidbody rb;
        public Transform target;


        public float followDelay = 1f;
        public float movementSpeed = 10f;
        public float rotationSpeed = 5f;



        Vector3 targetPos;
        Vector3 velocity;
        bool isPaused = true;
        bool isChasing;
        
        public MovementType movementType = MovementType.Smooth;

        public enum MovementType { 
            Linear = 0,
            Smooth = 1,
        }
        




        void Awake() {
            id = Random.Range(0, 1000000);
        }


        void FixedUpdate() {    
            
            if (isPaused) return;
                
            
            if (target != null) targetPos = target.position;


            // check if arrived
            if (Vector3.Distance(transform.position, targetPos) < 0.5f) {
                a_TargetReached?.Invoke(id, targetPos);
                return;
            }



            Vector3 newPosition = Vector3.zero;

            Vector3 targetDirection = targetPos - transform.position;
            float distance  = targetDirection.magnitude;
            float t_delay = Mathf.Min(distance, 10f) * followDelay;

            // <explainer>
            // Vector3 SmoothDamp(
            //         Vector3 current, 
            //         Vector3 target, 
            //         ref Vector3 currentVelocity, 
            //         float smoothTime, 
            //         float maxSpeed = Mathf.Infinity, 
            //         float deltaTime = Time.deltaTime); 
                
            if (movementType == MovementType.Smooth) {
                newPosition = Vector3.SmoothDamp(rb.position, targetPos, ref velocity, t_delay, movementSpeed);
            }

            if (movementType == MovementType.Linear)
                newPosition = transform.position + targetDirection.normalized*movementSpeed/100f;


            rb.MovePosition(newPosition);

            // Debug.Log($"dist: {distance} speed:{velocity.magnitude} delay: {t_delay}");

            if (distance > 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

        }

        




        public void ChastTarget(Transform target) {
            Unpause();
            this.target = target;
            targetPos = target.position;
        }

        public void MoveToPoint(Vector3 targetPos) {
            Unpause();
            target = null;
            this.targetPos = targetPos;
        }


        public void StopChase() {
            target = null;
            a_Stop?.Invoke();
        }

        public void Pause(float duration) {
            isPaused = true;
            Invoke(nameof(Unpause), duration);
        }

        public void Unpause() => isPaused = false;
    }
}