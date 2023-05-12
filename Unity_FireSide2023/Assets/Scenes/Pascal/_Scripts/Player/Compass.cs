using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal {


    public class Compass : MonoBehaviour
    {
        [SerializeField] Transform target;


        void Update() {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 v = lookRotation.eulerAngles;

            transform.eulerAngles = new Vector3(90, v.y+180f, 0);
        }
    }

}