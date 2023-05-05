using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MA_BoatAnimation : MonoBehaviour
{
    public float animSpeed = 1;
    public float animHeight = 1;
    private Vector3 centerPos;
    private void Awake(){
        centerPos = transform.localPosition;
        centerPos.y -= animHeight;
    }
    private float t; 
    // Update is called once per frame
    void Update(){
        t = (t + (Time.deltaTime * animSpeed)) % (Mathf.PI *2);
        transform.localPosition = new(centerPos.x, 
                                      centerPos.y + (Mathf.Cos(t) * animHeight),
                                      centerPos.z);
    }
}
