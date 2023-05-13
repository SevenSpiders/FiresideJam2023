using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {

    public class SafeZone : MonoBehaviour
    {
        
        [SerializeField] float healPerSecond = 50f;


        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("Player")) return;


        }

        void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player")) return;

            PlayerAttributes.boostTokens = PlayerAttributes.boostTokensMax;
            PlayerAttributes.Heal(healPerSecond*Time.deltaTime);
            
        }

    }
}