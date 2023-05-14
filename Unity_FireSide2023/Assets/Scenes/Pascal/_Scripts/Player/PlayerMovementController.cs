using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] float moveSpeedInit = 100f;
        [SerializeField] float speedBoostBonus = 3f;
        [SerializeField] float turnSpeed = 10f;
        [SerializeField] float groundDrag = 5f;
        [SerializeField] float t_boost = 1f;
        [SerializeField] float t_boostCooldown = 0.2f;
        [SerializeField] Rigidbody rb;
        [SerializeField] MeshRenderer sphere;
        [SerializeField] AudioManager audioManager;

        float horizontalInput;
        float verticalInput;
        bool mousePressed;
        bool boosted;   // is currently boosted state?
        Vector3 movementDir = Vector3.zero;


        float speedBoost = 1f;
        float moveSpeed => (moveSpeedInit + PlayerAttributes.speed) * Mathf.Max(speedBoost, 1f+PlayerAttributes.boostSpeed);


        float t;
        bool isOnBoostCooldown;


        void Start() {
            rb.drag = groundDrag;
        }


        
        void Update() {

            if (PlayerAttributes.isDead) return;
            if (PlayerAttributes.inputsDisabled) return;
            

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            
            UpdateBoost();

            // shield mechanic
            mousePressed = Input.GetMouseButton(0);
            sphere.enabled = mousePressed;
            PlayerAttributes.shieldActive = mousePressed;
        

            movementDir = Vector3.forward * verticalInput + Vector3.right * horizontalInput;
            if (movementDir.magnitude < 0.01f) return;

            speedBoost = boosted ? speedBoostBonus : 1f;
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


        

        void UpdateBoost() {
            
            // update boosted state
            if (boosted) {
                t += Time.deltaTime;
                if (t > t_boost) {
                    t = 0;
                    boosted = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) TryBoost();


        }


        void TryBoost() {
            if (boosted) return;
            if (isOnBoostCooldown) return;
            if (PlayerAttributes.boostTokens <= 0) return;

            PlayerAttributes.boostTokens -= 1;

            boosted = true;
            isOnBoostCooldown = true;
            audioManager.Play("SpeedBoost");
            Invoke(nameof(EndBoostCooldown), t_boostCooldown);
        }

        void EndBoostCooldown() => isOnBoostCooldown = false;
    }
}
