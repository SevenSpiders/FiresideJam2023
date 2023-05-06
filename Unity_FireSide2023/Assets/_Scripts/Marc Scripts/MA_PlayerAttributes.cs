using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MA_PlayerAttributes : MonoBehaviour
{
    public TextMeshProUGUI soulsUI;
    public float souls;

    public TextMeshProUGUI healthUI;
    public float health;
    public float recovery = 25f;
    public float regress = 5f;
    public float maxHealth;

    public TextMeshProUGUI coinsUI;
    public float coins;
    public float soulValueMultiplier = 2.5f;

    public Button sellButton;
    public TextMeshProUGUI sellButtonText;

    public bool isSafe = false;

    private void Awake()
    {
        if (sellButton == null)
            return;

        sellButton.onClick.AddListener (AddCoins);
    }

    private void OnValidate()
    {
        health = Mathf.Clamp((int)health, 0, (int)maxHealth);
        maxHealth = (int) maxHealth <= 0 ? 0 : (int)maxHealth;
        souls = (int)souls <= 0 ? 0 : (int)souls;
        coins = (int)coins <= 0 ? 0 : (int)coins;
    }
    private void Update()
    {

        if ( soulsUI == null || healthUI == null || coinsUI == null || sellButtonText == null)
            return;

        soulsUI.text = "Souls: "+((int)souls).ToString();
        healthUI.text = "Health: " + ((int)health).ToString() +" / " + ((int)maxHealth).ToString() + " (Regression: "+ regress.ToString() +" p/s)";
        coinsUI.text = "Coins: " + ((int)coins).ToString();
        sellButtonText.text = "Sell 1 soul for " + Mathf.RoundToInt(soulValueMultiplier).ToString() + " Coins";

        if (isSafe)
            Recover();
        else
            Regress();

    }

    public void AddCoins() {
        if( souls > 0) {
            coins += Mathf.RoundToInt(1 * soulValueMultiplier);
            souls--;
        }
    }

    public void Recover() {
        health = health < maxHealth ? health + (1 * recovery * Time.deltaTime) : maxHealth;
    }
    public void Regress(){
        health = health >= 0 ? health - (1 * regress * Time.deltaTime) : maxHealth;
    }
    public void Heal(float amount) {
        health = health + Mathf.Abs(amount) <= maxHealth ? health + Mathf.Abs(amount) : maxHealth; 
    }
    public void Damage(float amount) {
        health = health - Mathf.Abs(amount) >= 0 ? health - Mathf.Abs(amount) : 0;
    }
    public void EnableSellButton(bool enable) {
        sellButton.gameObject.SetActive(enable);
    }

}
