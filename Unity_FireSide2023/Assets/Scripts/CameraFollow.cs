using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;
    private Vector3 startPos;
    private Vector3 lerpedFollow;
    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, startPos + follow.position, Time.deltaTime * 5f);
        lerpedFollow = Vector3.Lerp(lerpedFollow, follow.position, Time.deltaTime * 5f);
        transform.LookAt(lerpedFollow);
    }
}
