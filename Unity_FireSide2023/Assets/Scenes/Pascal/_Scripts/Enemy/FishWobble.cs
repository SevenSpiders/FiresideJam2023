using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWobble : MonoBehaviour
{
    

    [SerializeField] Material woobleMat;
    [SerializeField, Tooltip("Wobble Frequency")] float f_wobble = 20f;
    [SerializeField] float maxSpeed = 40f;

    float frequency;
    Vector3 previousPosition;
    float speed;

    void Start() {
       
    }

    void Update() {
        Vector3 currentPosition = transform.position;
        speed = (currentPosition - previousPosition).magnitude / Time.deltaTime;
        previousPosition = currentPosition;
        
        float _f = (1f - 1f/ (speed/maxSpeed+1f) )* f_wobble;
        frequency = Mathf.Lerp(frequency, _f, 0.01f);
        woobleMat.SetFloat("_Frequency", frequency);
        // Debug.Log($"speed: {speed}, {_f}");

    }

    
    
}
