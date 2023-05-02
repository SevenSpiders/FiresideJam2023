using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class altCamController : MonoBehaviour
{
    Camera cam => GetComponent<Camera>();
    private int layerMask => ~LayerMask.GetMask("Player");


    public Transform player;
    public float speedX = 4f, speedY = 1f;
    public float maxAngle;
    public float camRadius = 2f;
    public float camMaxDistance = 5f;
    public float camAdjustSpeed = 10f;
    private float curX = 0f, curY = 30f;
    private float distance = 2f;
    private bool camHit;
    private bool camBlocked;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        camHit = Physics.CheckSphere(transform.position, camRadius, layerMask);
        camBlocked = Physics.Raycast(transform.position, -transform.forward, camRadius + 1f);
    }
    private void Update()
    {
        if (cam == null || player == null)
            return;
        curX += Input.GetAxis("Mouse X") * speedX;
        curY += Input.GetAxis("Mouse Y") * speedY;
        curY = Mathf.Clamp(curY, -maxAngle, maxAngle);

        distance = camHit ? Mathf.Lerp(distance, 0, Time.deltaTime * camAdjustSpeed) : camBlocked ? distance : Mathf.Lerp(distance, camMaxDistance, Time.deltaTime * camAdjustSpeed);
    }
    void LateUpdate()
    {
        if (cam == null || player == null)
            return;


        Vector3 dir =  new (0,0,-distance);
        Quaternion rot = Quaternion.Euler(curY, curX, 0);
        transform.position = player.position + rot * dir;
        transform.LookAt(player);
    }
}
