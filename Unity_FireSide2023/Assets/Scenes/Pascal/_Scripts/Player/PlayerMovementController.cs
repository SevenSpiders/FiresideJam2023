using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] float moveSpeedInit = 100f;
        [SerializeField] float speedBoostBonus = 5f;
        [SerializeField] float turnSpeed = 10f;
        [SerializeField] float groundDrag = 5f;

        float horizontalInput;
        float verticalInput;
        bool boostPressed;
        Vector3 movementDir = Vector3.zero;


        float speedBoost = 1f;
        float moveSpeed => moveSpeedInit * Mathf.Max(speedBoost, 1f+PlayerAttributes.boostSpeed);

        void Start() {
            rb.drag = groundDrag;
        }
        
        void Update() {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            boostPressed = Input.GetKey(KeyCode.Space);
        

            movementDir = Vector3.forward * verticalInput + Vector3.right * horizontalInput;
            if (movementDir.magnitude < 0.01f) return;

            speedBoost = boostPressed ? speedBoostBonus : 1f;
            rb.AddForce(movementDir.normalized * moveSpeed, ForceMode.Force);

            Quaternion targetRotation = Quaternion.LookRotation(movementDir.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

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