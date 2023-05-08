using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {
    

    public class FollowPlayer : MonoBehaviour {

        public Transform target;
        public float followDelay = 1f;
        public float rotationSpeed = 5f;

        private Vector3 velocity = Vector3.zero;

        private void FixedUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, followDelay);

            Vector3 targetDirection = target.position - transform.position;
            if (targetDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}