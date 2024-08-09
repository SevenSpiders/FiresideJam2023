using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pascal
{
    public class UI_Stats : MonoBehaviour
    {
        [SerializeField] TMP_Text healthStat;
        [SerializeField] TMP_Text speedStat;
        [SerializeField] TMP_Text shieldStat;

        void Update() {
            healthStat.text = "Health: " + PlayerAttributes.maxHealth.ToString();
            speedStat.text = "Speed: " + PlayerAttributes.speed.ToString();
            shieldStat.text = "Shields: " + PlayerAttributes.shieldLevel.ToString();
        }
    }
    
}
