using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {
    public class SoulDepositZone : MonoBehaviour
    {
        
        [SerializeField] VisualEffect vfx;

        float t;
        float t_tick = 0.5f;
        float healPerSecond = 50f;


        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("Player")) return;
            int souls = PlayerAttributes.souls;

            if (souls == 0) return;


            PlayerAttributes.souls = 0;
            PlayerAttributes.coins += souls * 1;

            // scale vfx with souls
            vfx.Play();
            t = 0;
        }

        void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player")) return;

            t += Time.deltaTime;
            if (t < t_tick) return;

            t = 0;

            // PlayerAttributes.Heal(healPerSecond*t_tick);
        }

        // void OnTriggerExit(Collider other) {
            
        //     if (!other.CompareTag("Player")) return;
            
        // }
    }
}