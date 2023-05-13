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
        // [SerializeField] TMP_Text armorStat;

        void Update() {
            healthStat.text = "Health: " + PlayerAttributes.maxHealth.ToString();
            speedStat.text = "Speed: " + PlayerAttributes.speed.ToString();
        }
    }
    
}
