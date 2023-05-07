using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal{
    

    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] float speed = 5.0f;
        [SerializeField] float turnSpeed = 10f;

        void Update() {

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            
            Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput) * speed * Time.deltaTime;

            if (movement.magnitude < 0.01f) return; 

            transform.position += movement;

            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

}
