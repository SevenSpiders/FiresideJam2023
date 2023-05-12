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


    [SerializeField] TMP_Text countSouls;
    public TextMeshProUGUI coinsUI;


    [SerializeField] Image animationFill;
    [SerializeField] Image fireBar;
    [SerializeField] Pascal.UI_BuyScreen buyScreen;
    [SerializeField] GameObject gameOverScreen;


    float hp = 1f; // health percentage



    void Awake() {
        A_ShowBuyScreen += OpenBuyScreen;
        A_HideBuyScreen += CloseBuyScreen;
        A_ShowGameOver += ShowGameOver;
        A_HideGameOver += HideGameOver;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            CloseBuyScreen();

        coinsUI.text = Mathf.RoundToInt(PlayerAttributes.coins).ToString();
        fireBar.fillAmount = PlayerAttributes.health / PlayerAttributes.maxHealth;
        UpdateSouls();
    }

    void UpdateHealthBar() {
        float newHP = PlayerAttributes.health / PlayerAttributes.maxHealth;
        if (hp != newHP) StartCoroutine(AnimateFillAmountCoroutine(newHP));
        hp = newHP;
        fireBar.fillAmount = hp;
    }

    IEnumerator AnimateFillAmountCoroutine(float targetFillAmount) {

        animationFill.fillAmount = hp;
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



    void OpenBuyScreen() => buyScreen.gameObject.SetActive(true);
    void CloseBuyScreen() => buyScreen.gameObject.SetActive(false);
    void ShowGameOver() => gameOverScreen.SetActive(true);
    void HideGameOver() => gameOverScreen.SetActive(false);


}
