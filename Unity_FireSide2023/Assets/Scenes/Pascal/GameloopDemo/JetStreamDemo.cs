using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {


    public class JetStreamDemo : MonoBehaviour
    {
        [SerializeField] Material mat;
        [SerializeField] float speed;
        float offset;

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update() {
            offset += speed * Time.deltaTime;
            mat.SetVector("_Offset", new Vector4(0, offset, 0,0));
        }
    }


}


