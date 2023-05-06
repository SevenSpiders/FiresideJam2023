using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] float moveSpeed;
        [SerializeField] float groundDrag = 5f;

        float horizontalInput;
        float verticalInput;
        Vector3 movementDir = Vector3.zero;

        void Start() {
            rb.drag = groundDrag;
        }
        
        void Update() {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        

            movementDir = transform.forward * verticalInput + transform.right * horizontalInput;
            rb.AddForce(movementDir.normalized*moveSpeed, ForceMode.Force);
            SpeedControl();
        }

        void SpeedControl() {
            Vector3 velFlat = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (velFlat.magnitude > moveSpeed) {
                Vector3 vel = velFlat.normalized *moveSpeed;
                rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
            }
        }
    }
}
