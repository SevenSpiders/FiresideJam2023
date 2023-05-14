using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {

    public class SafeZone : MonoBehaviour
    {
        
        [SerializeField] float healPerSecond = 0.2f;
        [SerializeField] AudioManager audioManager;


        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("Player")) return;

            if(PlayerAttributes.HP < 0.99f) audioManager.Play("Heal");
        }

        void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player")) return;

            PlayerAttributes.boostTokens = PlayerAttributes.boostTokensMax;
            PlayerAttributes.shieldTokens = PlayerAttributes.shieldTokensMax;

            float _heal = healPerSecond * PlayerAttributes.maxHealth;
            PlayerAttributes.Heal(_heal*Time.deltaTime);
            if (PlayerAttributes.HP > 0.99f) audioManager.Stop("Heal", fade: true);
        }

    }
}