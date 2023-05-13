using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {
    public class SoulDepositZone : MonoBehaviour
    {
        
        [SerializeField] VisualEffect vfx;
        [SerializeField] AudioManager audioManager;


        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("Player")) return;
            int souls = PlayerAttributes.souls;

            if (souls == 0) return;

            // sell
            float coins = (float) PlayerAttributes.souls * PlayerAttributes.coinsPerSoul;
            PlayerAttributes.coins += (int) coins;
            PlayerAttributes.souls = 0;

            // scale vfx with souls
            vfx.Play();
            audioManager.Play("Coins");
        }

        void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player")) return;

           
        }

        
    }
}