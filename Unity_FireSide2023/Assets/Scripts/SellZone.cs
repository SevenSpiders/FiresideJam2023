using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellZone : MonoBehaviour
{

    public Button sellButton;
    public TextMeshProUGUI sellText;

    private void Awake() {
        sellButton.onClick.AddListener(()=>PlayerAttributes.AddCoins());
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || sellButton == null || sellText == null)
            return;

        sellButton.gameObject.SetActive(true);
        sellText.text = "Sell 1 Souls For " + Mathf.RoundToInt(PlayerAttributes.soulValueMultiplier).ToString() + " Coins";

    }
    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player") || sellButton == null || sellText == null)
            return;

        sellButton.gameObject.SetActive(false);
    }

}
