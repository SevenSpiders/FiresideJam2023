using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI soulsUI;
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI coinsUI;
    public TextMeshProUGUI sellButtonText;

    private void Update()
    {
        if ( soulsUI == null || healthUI == null || coinsUI == null || sellButtonText == null)
            return;

        soulsUI.text = "Souls: "+(Mathf.RoundToInt(PlayerAttributes.souls).ToString());
        healthUI.text = "Health: " + Mathf.RoundToInt(PlayerAttributes.health).ToString() +" / " +  Mathf.RoundToInt(PlayerAttributes.maxHealth).ToString() +  " (Regression: "+ PlayerAttributes.regress.ToString() +" p/s)";
        coinsUI.text = "Coins: " + Mathf.RoundToInt(PlayerAttributes.coins).ToString();
        sellButtonText.text = "Sell 1 soul for " + Mathf.RoundToInt(PlayerAttributes.soulValueMultiplier).ToString() + " Coins";
    }

}