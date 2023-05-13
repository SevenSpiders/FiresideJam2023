using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Pascal {

    public class UI_GameOver : MonoBehaviour
    {
        [SerializeField] TMP_Text coinCounter;
        [SerializeField] TMP_Text soulCounter;
        


        void OnEnable() {
            coinCounter.text = $"-{PlayerAttributes.coinLoss}";
            soulCounter.text = $"-{PlayerAttributes.soulLoss}";
        }
    }

}
