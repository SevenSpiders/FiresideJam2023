using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal {

    public class CameraController : MonoBehaviour
    {
        public Transform playerTransform;

        Vector3 offset;

        void Start() {
            offset = transform.position - playerTransform.position;
        }

        void Update() {
            transform.position = playerTransform.position + offset;
        }
    }

}