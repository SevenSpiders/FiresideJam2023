using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal {


    public class HurtBox : MonoBehaviour
    {
        [SerializeField] int damage;

        float t_tick = 0.5f;
        float t;

        CharacterHealth playerHealth;

       

        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;

            DealDamage(other);
        }

        void OnTriggerStay(Collider other) {
            // Debug.Log($"tick {t}");
            if (!other.CompareTag("Player")) return;

            t += Time.deltaTime;
            if (t < t_tick) return;

            t = 0;

            DealDamage(other);
        }



        protected virtual void DealDamage(Collider other) {
            if (playerHealth == null) 
                playerHealth = other.GetComponent<CharacterHealth>();
                if (playerHealth == null) return;
            
            playerHealth.TakeDamage(damage);
        }



        private void OnTriggerExit(Collider other) {
           
        }

    }
}