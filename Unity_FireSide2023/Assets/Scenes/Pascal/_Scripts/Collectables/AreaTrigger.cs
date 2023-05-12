using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal {
    public class AreaTrigger : MonoBehaviour
    {
        public System.Action a_Trigger;

        void OnTriggerEnter(Collider other) {
            
            if (!other.CompareTag("Player")) return;
            a_Trigger?.Invoke();
        }
    }
}

