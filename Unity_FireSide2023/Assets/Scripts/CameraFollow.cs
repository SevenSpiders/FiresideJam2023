using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;
    public float followSpeed;
    private Vector3 startPos;
    private Vector3 lerpedFollow;

    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        // transform.position = Vector3.Lerp(transform.position, follow.position + offset, Time.deltaTime * followSpeed);
        // lerpedFollow = Vector3.Lerp(lerpedFollow, follow.position, Time.deltaTime * followSpeed);
        // transform.LookAt(lerpedFollow);
    }
}
