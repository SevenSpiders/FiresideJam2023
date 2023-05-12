using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {

    public class ShieldJitter : MonoBehaviour
    {
        [SerializeField] float frequency = 2f;
        [SerializeField] float amplitude = 1f;

        Vector3 scaleInit;
        float t;

        void Start() {
            scaleInit = transform.localScale;
        }
        
        void Update() {
            t += Time.deltaTime;
            Vector3 scale = scaleInit + Vector3.one* Mathf.Sin(t * frequency) *amplitude;
            transform.localScale = scale;
        }
    }

}
