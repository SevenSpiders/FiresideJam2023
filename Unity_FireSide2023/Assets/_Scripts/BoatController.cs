using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoatController : PhysicsObject
{
    [Header("Boat Controller Stuff")]
    public Light pointLight;
    public Light spotLight;

    private float maxSpeed;
    private Vector3 lookAtVec = Vector3.forward;
    private Vector3 moveVec = Vector3.forward;
    


    void Update()
    {
        // Enables the spotlight on left mousebutton to mouse position
        if (spotLight == null)
            return;

        spotLight.intensity = Input.GetButton("Fire1") ?
            Mathf.Lerp(spotLight.intensity, 200, Time.deltaTime * 10) :
            Mathf.Lerp(spotLight.intensity, 0, Time.deltaTime * 10);

        float distToPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y, distToPlayer));

        Vector3 direction = mouseWorldPos - transform.position;
        direction.y = 0;
        direction.Normalize();

        spotLight.transform.forward = Input.GetButton("Fire1") && Vector3.Dot(transform.forward, direction) > 0 ?
            Vector3.Lerp(spotLight.transform.forward, direction, Time.deltaTime * 10) :
            Vector3.Lerp(spotLight.transform.forward, transform.forward, Time.deltaTime * 10);

        //Dim point light with current health
        if (pointLight == null)
            return;

        pointLight.intensity = Mathf.Lerp(0,100,PlayerAttributes.health / PlayerAttributes.maxHealth);
        pointLight.range = Mathf.Lerp(0, 15, PlayerAttributes.health / PlayerAttributes.maxHealth);


        // Disable movement 
        if (!PlayerAttributes.movementEnabled) {
            CharacterMove(Vector3.Lerp(Rb.velocity,Vector3.zero,Time.deltaTime * 10));
            CharacterLookAt(lookAtVec);
            moveVec = Vector3.zero;
            return;
        }

        
        // Set the speed to sprint or walk speed (only when grounded so player keeps his speed after jump)
        if (IsGrounded)
            maxSpeed = Input.GetButton("Jump") ? Mathf.Lerp(maxSpeed, PlayerAttributes.sprintSpeed, Time.deltaTime * 5) : Mathf.Lerp(maxSpeed, PlayerAttributes.speed, Time.deltaTime * 5);

        // Get the input magnitude from Raw Input
        float verInputRaw = Input.GetAxisRaw("Vertical");
        float horInputRaw = Input.GetAxisRaw("Horizontal");
        float curInput = new Vector2(verInputRaw, horInputRaw).normalized.magnitude;

        // Get the cameras forward vector
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // Get the cameras right vector
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Get the look at position for the player
        Vector3 targetPos = transform.position + (camForward * verInputRaw + camRight * horInputRaw);
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




    }

    public override void OnGroundContact(Vector3 contactPoint)
    {
        // add water splash sound and water splash particles
    }

}
