using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class altCharController : Character_Controller
{

    private Vector3 lookDir;
    private Vector3 moveDir;


    public Transform cam;
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce = 50f;

    public override void OnGroundContact(Vector3 contactPoint)
    {

    }

    private void Update()
    {
        float horInputRaw = Input.GetAxisRaw("Horizontal");
        float verInputRaw = Input.GetAxisRaw("Vertical");
        bool inputPressed = horInputRaw != 0 || verInputRaw != 0;
       

        lookDir = inputPressed ? horInputRaw * cam.right + verInputRaw * cam.forward : lookDir;
        moveDir = (horInputRaw * cam.right + verInputRaw * cam.forward).normalized * moveSpeed;
        CharacterLookAt(lookDir);
        CharacterMove(moveDir);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            CharacterJump(jumpHeight);

        if (Input.GetMouseButtonDown(0))
            Shoot();
        
    }

    void Shoot(float lifeTime = 5f)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward * radius, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
        projectile.AddComponent<Bullet>().lifeTime = lifeTime;
    }

}
