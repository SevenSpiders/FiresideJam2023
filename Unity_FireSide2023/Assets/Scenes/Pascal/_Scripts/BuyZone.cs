using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pascal {
    public class BuyZone : MonoBehaviour
    {
        public System.Action a_Trigger;

            void OnTriggerEnter(Collider other) {
                
                if (!other.CompareTag("Player")) return;
                HUD.A_ShowBuyScreen?.Invoke();
            }

            void OnTriggerExit(Collider other) {
                
                if (!other.CompareTag("Player")) return;
                HUD.A_HideBuyScreen?.Invoke();
            }
    }
}