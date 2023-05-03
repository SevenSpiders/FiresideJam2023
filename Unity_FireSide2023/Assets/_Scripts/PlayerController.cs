using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



/*

    https://www.youtube.com/watch?v=UCwwn2q4Vys
*/


public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance {get; private set;}

    public static Vector2 movementInput;
    // public static bool jumpInput;
    // public static bool shootInput;




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


    readonly Collider[] _groundHits = new Collider[1];
    float groundMargin = 0.2f;
    Vector3 groundPos => transform.position + Vector3.down* playerHeight*0.5f;
    bool isGrounded => Physics.OverlapSphereNonAlloc(groundPos, groundMargin, _groundHits, groundMask) > 0;

    

    Vector3 movementDir;
    float jumpCooldown = 0.1f;
    [SerializeField] bool readyToJump = true;

    

    Rigidbody rb;
    PlayerInput input;







    // -------- START ----------

    void Awake() {

        if (Instance != null && Instance != this) Destroy(this); 
        else Instance = this; 

        input = new PlayerInput();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        #if UNITY_EDITOR
        DrawDebugger();
        #endif
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
        // isGrounded = Physics.Raycast(groundPos, Vector3.down, groundMargin, groundMask);
        rb.drag = isGrounded ? groundDrag : 0;
    }






    // ---------- HANDLERS ------------
    void HandleMovmentInput(InputAction.CallbackContext value) {
        movementInput = value.ReadValue<Vector2>();
    }

    void HandleMovementCanceled(InputAction.CallbackContext value) {
        movementInput = Vector2.zero;
    }

    void HandleJumpInput(InputAction.CallbackContext value) {
        Jump();
    }

    void HandleShootInput(InputAction.CallbackContext value) {
        Shoot();
    }



    void Move() {

        movementDir = transform.forward * movementInput.y + transform.right * movementInput.x;
        float speed = isGrounded ? movementSpeed * 10f : airSpeed * 10f;
        rb.AddForce(movementDir*speed, ForceMode.Force);
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


    // ----------- DEBUG -------------

    public void Test() { // called via Editor_PlayerController
        Debug.LogWarning("Debug!");
    }

    void DrawDebugger() {
        Vector3 start = projectileSpawnPoint.position;
        Vector3 end = start + projectileSpawnPoint.forward *50f;

        Debug.DrawLine(start, end, Color.red);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundPos, groundMargin);
    }


    
}


