using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal {


    public class Oscilator : MonoBehaviour
    {
        [SerializeField] float amplitude = 1.0f;
        [SerializeField] float frequency = 1.0f;
        
        Vector3 startingPosition;
        
        void Start() {
            startingPosition = transform.localPosition;
        }
        
        void Update() {
            Vector3 newPosition = startingPosition;
            newPosition.x += amplitude * Mathf.Sin(Time.time * frequency);
            transform.localPosition = newPosition;
        }
    }

}
