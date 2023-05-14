using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal {


    public class TrailWiggler : MonoBehaviour
    {
        [SerializeField] float amplitude = 1.0f;
        [SerializeField] float frequency = 1.0f;

        [SerializeField] float speedMax = 80f;


        
        float speed;
        Vector3 startingPosition;
        Vector3 previousPosition;
        
        void Start() {
            startingPosition = transform.localPosition;
        }
        
        void Update() {

            // Update Speed
            Vector3 currentPosition = transform.position;
            float _speed = (currentPosition - previousPosition).magnitude / Time.deltaTime;
            speed = Mathf.Lerp(speed, _speed, 0.2f);
            previousPosition = currentPosition;

            float _f = speed/ speedMax;
            float _freq = Mathf.Max(0, frequency - _f);
            float _amp  = Mathf.Max(0, amplitude - _f);

            Vector3 newPosition = startingPosition;
            newPosition.x += _amp * Mathf.Sin(Time.time * _freq);
            transform.localPosition = newPosition;
        }

    }

}
