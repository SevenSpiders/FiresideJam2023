using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {
    

    public class FollowPlayer : MonoBehaviour {

        [SerializeField] Transform target;
        [SerializeField] float followDelay = 1f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] Material wobbleMaterial;
        [SerializeField] float f_wobble = 1f;


        // [SerializeField] Transform fish;
        // [SerializeField] float f_fish = 0.5f;
        // [SerializeField] float deltaY = 2f;
        // [SerializeField] float y0 = -4f;
        

        Vector3 velocity = Vector3.zero;
        float runningAvg;
        float t;


        void FixedUpdate() {    

            t += Time.deltaTime;        

            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, followDelay);

            runningAvg = runningAvg*0.8f + velocity.magnitude*0.2f;
            wobbleMaterial.SetFloat("_Frequency", runningAvg*f_wobble);

            Vector3 targetDirection = target.position - transform.position;
            if (targetDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }



            // Vector3 pos = fish.localPosition;
            // pos.y = Mathf.Sin(t*f_fish)*deltaY + y0;
            // fish.localPosition = pos;
        }
    }
}