using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {
    

    public class ChasePlayer : MonoBehaviour {

        public System.Action a_Stop;
        public Transform target;
        public bool isChasing;
        public bool isPaused = true;

        [SerializeField] float followDelay = 1f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] Rigidbody rb;

        Vector3 targetPos;
        

        Vector3 velocity = Vector3.zero;
        float runningAvg;



        void Awake() {

        }


        void FixedUpdate() {    
            
            if (isPaused) return;
                
            
            if (target != null) targetPos = target.position;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, followDelay);

            runningAvg = runningAvg*0.99f + velocity.magnitude*0.01f;
            // wobbleMaterial.SetFloat("_Frequency", runningAvg*f_wobble);

            Vector3 targetDirection = targetPos - transform.position;
            if (targetDirection.magnitude > 0.1f) {
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