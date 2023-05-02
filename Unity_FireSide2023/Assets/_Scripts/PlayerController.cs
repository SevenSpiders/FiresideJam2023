using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*

    https://www.youtube.com/watch?v=UCwwn2q4Vys
*/

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float airSpeed = 4f;
    [SerializeField] float groundDrag = 5f;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileForce = 50f;

    [Header("Ground")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float playerHeight = 1f;



    
    
    Rigidbody rb;

    Vector2 movementInput;
    Vector3 movementDir;
    float jumpCooldown = 0.1f;
    bool readyToJump = true;
    public bool isGrounded;

    PlayerInput input;


    void Awake() {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody>();
    }




    void FixedUpdate() {
        if (movementInput.magnitude > 0f) {
            Move();
            Rotate();
        }
        CheckGrounded();
    }

















    void OnEnable() {
        input.Enable();
        input.Player.Movement.performed += HandleMovmentInput;
        input.Player.Movement.canceled += HandleMovementCanceled;
        input.Player.Jump.performed += HandleJumpInput;
        input.Player.Shoot.performed += HandleShootInput;
    }

    void OnDisable() {
        input.Disable();
        input.Player.Movement.performed -= HandleMovmentInput;
        input.Player.Movement.canceled -= HandleMovementCanceled;
        input.Player.Jump.performed -= HandleJumpInput;
        input.Player.Shoot.performed -= HandleShootInput;
    }


    void CheckGrounded() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight*0.5f + 0.2f, groundMask);
        rb.drag = isGrounded ? groundDrag : 0;
    }


    // ---------- HANDLERS ------------
    void HandleMovmentInput(InputAction.CallbackContext value) {
        movementInput = value.ReadValue<Vector2>();
    }

    void HandleMovementCanceled(InputAction.CallbackContext value) {
        movementInput = Vector3.zero;
    }

    void HandleJumpInput(InputAction.CallbackContext value) {
        Jump();
    }

    void HandleShootInput(InputAction.CallbackContext value) {
        Shoot();
    }



    void Move() {
        movementDir = transform.forward * movementInput.y + transform.right * movementInput.x;
        // Vector3 newPos = transform.position + (movementDir * movementSpeed * Time.deltaTime);
        // rb.MovePosition(newPos);
        float multiplier = isGrounded ? 10f : airSpeed;
        rb.AddForce(movementDir*movementSpeed*multiplier, ForceMode.Force);
        SpeedControl();
    }

    void SpeedControl() {
        Vector3 velFlat = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (velFlat.magnitude > movementSpeed) {
            Vector3 vel = velFlat.normalized *movementSpeed;
            rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
        }
    }

    void Rotate() {
    
            // Vector3 movement = transform.forward * movementInput.y + transform.right * movementInput.x;
            // Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movementInput.x, 0f, movementInput.y));
        Quaternion targetRotation = Quaternion.LookRotation(movementDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        
    }

    void Jump() {
        if (!readyToJump) return;
        if (!isGrounded) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        readyToJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    void ResetJump() {
        readyToJump = true;
    }

    void Shoot() {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);
    }




}


