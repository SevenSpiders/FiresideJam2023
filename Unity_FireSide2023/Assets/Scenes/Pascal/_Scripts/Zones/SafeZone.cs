using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {

    public class SafeZone : MonoBehaviour
    {
        
        [SerializeField] float healPerSecond = 50f;

        float t;
        float t_tick = 0.5f;


        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("Player")) return;
            t = 0;
        }

        void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player")) return;

            t += Time.deltaTime;
            if (t < t_tick) return;

            t = 0;

            PlayerAttributes.Heal(healPerSecond*t_tick);
            
        }

    }
}