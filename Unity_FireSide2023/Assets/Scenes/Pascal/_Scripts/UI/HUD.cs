using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class HUD : MonoBehaviour
{

    public static System.Action A_ShowBuyScreen;
    public static System.Action A_HideBuyScreen;
    public static System.Action A_ShowGameOver;
    public static System.Action A_HideGameOver;


    // Other Menus:
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject hudScreen;
    [SerializeField] Pascal.UI_BuyScreen buyScreen;




    [SerializeField] TMP_Text countSouls;
    [SerializeField] TMP_Text countCoins;


    [SerializeField] Image fireBar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image animationFill;
    [SerializeField] List<Pascal.UI_Icon> boostTokens;



    float hp = 1f; // health percentage






    // --------- START ---------

    void Awake() {
        A_ShowBuyScreen += OpenBuyScreen;
        A_HideBuyScreen += CloseBuyScreen;
        A_ShowGameOver += ShowGameOver;
        A_HideGameOver += HideGameOver;
    }

    void Start() {
        UpdateBoostTokens();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            CloseBuyScreen();

        countCoins.text = Mathf.RoundToInt(PlayerAttributes.coins).ToString();
        // fireBar.fillAmount = PlayerAttributes.health / PlayerAttributes.maxHealth;
        UpdateHealthBar();
        UpdateSouls();
        UpdateBoostTokens();
    }

    void UpdateHealthBar() {

        healthText.text = $"{(int)PlayerAttributes.health}/ {(int)PlayerAttributes.maxHealth}"; 
        float newHP = PlayerAttributes.health / PlayerAttributes.maxHealth;
        if (hp != newHP) StartCoroutine(AnimateFillAmountCoroutine(hp, newHP));
        fireBar.fillAmount = newHP;
        hp = newHP;
    }

    IEnumerator AnimateFillAmountCoroutine(float from, float targetFillAmount) {

        animationFill.fillAmount = from;
        float duration = 0.2f;
        float changePerFrame = targetFillAmount / duration * Time.deltaTime;

        while (animationFill.fillAmount < targetFillAmount) {
            animationFill.fillAmount += changePerFrame;
            yield return null;
        }
    }


    


    void UpdateSouls() {
        countSouls.text = PlayerAttributes.souls.ToString();
    }

    void UpdateBoostTokens() {
        for (int i=0; i<boostTokens.Count; i++) {
            boostTokens[i].gameObject.SetActive( i< PlayerAttributes.boostTokensMax);
            boostTokens[i].SetAvailable( i < PlayerAttributes.boostTokens);
        }
    }

    void OpenBuyScreen() => buyScreen.gameObject.SetActive(true);
    void CloseBuyScreen() => buyScreen.gameObject.SetActive(false);

    void ShowGameOver() {
        gameOverScreen.SetActive(true);
        hudScreen.SetActive(false);
    }
    void HideGameOver() {
        gameOverScreen.SetActive(false);
        hudScreen.SetActive(true);
    }


}
