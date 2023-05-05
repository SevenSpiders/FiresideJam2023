using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MA_BoatController : MA_Boat
{

    [Header("Movement Settings")]
    public float speed = 8f;
    public float sprintSpeed = 12f;
    public float accelRate;
    public float deccelRate;

    private float maxSpeed;
    private Vector3 lookAtVec;
    private Vector3 moveVec;
    
    [Header("Particle Settings")]
    // To Access from other classes
    public static bool movementEnabled = true;


    // Update is called once per frame

    void Update()
    {
        if (!movementEnabled)
            return;

        // Set the speed to sprint or walk speed (only when grounded so player keeps his speed after jump)
        if (IsGrounded)
            maxSpeed = Input.GetButton("Fire1") ? Mathf.Lerp(maxSpeed, sprintSpeed, Time.deltaTime * 5) : Mathf.Lerp(maxSpeed, speed, Time.deltaTime * 5);

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
        lookAtVec = curInput > 0 ? (targetPos - transform.position).normalized : lookAtVec;
        CharacterLookAt(lookAtVec);

        // Get to move vector from input
        moveVec = curInput > 0 ? Vector3.Lerp(moveVec, curInput * maxSpeed * transform.forward, Time.deltaTime * accelRate) : Vector3.Lerp(moveVec,Vector3.zero,Time.deltaTime * deccelRate);
        CharacterMove(moveVec);

    }

    public override void OnGroundContact(Vector3 contactPoint)
    {
        // add water splash sound and water splash particles
    }

}
