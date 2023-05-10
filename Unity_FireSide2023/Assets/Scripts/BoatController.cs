using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoatController : PhysicsObject
{
    [Header("Boat Controller Stuff")]
    public bool moveWithMouse = false;
    //public GameObject projectile;

    private float maxSpeed;
    private Vector3 lookAtVec = Vector3.forward;
    private Vector3 moveVec = Vector3.forward;

    void Update()
    {


        // Disable movement 
        if (!PlayerAttributes.movementEnabled) {
            CharacterMove(Vector3.Lerp(Rb.velocity,Vector3.zero,Time.deltaTime * 10));
            CharacterLookAt(lookAtVec);
            moveVec = Vector3.zero;
            return;
        }

        Vector3 mouseWorldPos = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
            mouseWorldPos = hit.point;

        Vector3 mouseDir = mouseWorldPos - transform.position;
        mouseDir.y = 0;
        mouseDir.Normalize();

        // Set the speed to sprint or walk speed (only when grounded so player keeps his speed after jump)
        maxSpeed = Input.GetButton("Jump") ? Mathf.Lerp(maxSpeed, PlayerAttributes.sprintSpeed + PlayerAttributes.boostSpeed, Time.deltaTime * 5) : Mathf.Lerp(maxSpeed, PlayerAttributes.speed + PlayerAttributes.boostSpeed, Time.deltaTime * 5);
        PlayerAttributes.boostCooldown = PlayerAttributes.boostCooldown > 0 ? PlayerAttributes.boostCooldown - Time.deltaTime : 0;
        PlayerAttributes.boostSpeed = PlayerAttributes.boostCooldown > 0 || PlayerAttributes.boostSpeed <= 0 ? PlayerAttributes.boostSpeed : PlayerAttributes.boostSpeed - (PlayerAttributes.accelRate * Time.deltaTime);
        Debug.Log("Boost Speed: " + PlayerAttributes.boostSpeed + "    Boost CoolDown: " + PlayerAttributes.boostCooldown);
        

        // Get the input magnitude from Raw Input
        float verInputRaw = Input.GetAxisRaw("Vertical");
        float horInputRaw = Input.GetAxisRaw("Horizontal");
        float curInput = moveWithMouse ?Input.GetButton("Fire2") ? 1 : 0 : new Vector2(verInputRaw, horInputRaw).normalized.magnitude;

        // Get the cameras forward vector
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // Get the cameras right vector
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Get the look at position for the player
        
        Vector3 targetPos = moveWithMouse ? transform.position + mouseDir :transform.position + (camForward * verInputRaw + camRight * horInputRaw);
        float t = (Mathf.Abs(2 - Vector3.Distance((targetPos - transform.position).normalized, transform.forward)) + .5f) * Time.deltaTime * PlayerAttributes.rotationRate;
        lookAtVec = curInput > 0 ? Vector3.Slerp(lookAtVec, (targetPos - transform.position).normalized, t) : lookAtVec;
        CharacterLookAt(lookAtVec);

        // Get to move vector from input
        moveVec = curInput > 0 ? Vector3.Lerp(moveVec, curInput * maxSpeed * transform.forward, Time.deltaTime * PlayerAttributes.accelRate) : Vector3.Lerp(moveVec,Vector3.zero,Time.deltaTime * PlayerAttributes.deccelRate);
        CharacterMove(moveVec);

        if (PlayerAttributes.isSafe)
            PlayerAttributes.Recover();
        else
            PlayerAttributes.Regress();

        /* Shooting



        if (projectile == null || projectile.GetComponent<Projectile>() == null)
            return;

        if (Input.GetButtonDown("Fire2")) {
            GameObject curProj = Instantiate(projectile);
            curProj.GetComponent<Projectile>().ShootProjectile(transform.position + Vector3.up, mouseWorldPos);
        }
        */


    }

    public override void OnGroundContact(Vector3 contactPoint)
    {
        // add water splash sound and water splash particles
    }



}
