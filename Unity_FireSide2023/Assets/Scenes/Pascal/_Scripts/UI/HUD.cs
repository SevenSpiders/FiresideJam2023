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

    public static System.Action A_ShowHUD;
    public static System.Action A_HideHUD;
    public static System.Action A_ShowDialog;
    public static System.Action A_HideDialog;
    public static System.Action<string> A_Dialog;
    public static System.Action<string> A_Prompt;



    // Other Menus:
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject hudScreen;
    [SerializeField] Pascal.UI_BuyScreen buyScreen;
    [SerializeField] Pascal.UI_Dialog dialogScreen;




    [SerializeField] TMP_Text promptText;


    [SerializeField] TMP_Text countSouls;
    [SerializeField] TMP_Text countCoins;
    [SerializeField] TMP_Text countShields;


    [SerializeField] Image fireBar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image animationFill;
    [SerializeField] List<Pascal.UI_Icon> boostTokens;

    [SerializeField] AudioManager audioManager;


    float hp = 1f; // health percentage
    Tween blinkTween;
    Color fireColor1;
    bool isLowHealth;
    public Color fireColor2;
    public float t_blink;





    // --------- START ---------

    void Awake() {
        A_ShowBuyScreen += OpenBuyScreen;
        A_HideBuyScreen += CloseBuyScreen;
        A_ShowGameOver += ShowGameOver;
        A_HideGameOver += HideGameOver;
        A_ShowHUD += ShowHUD;
        A_HideHUD += HideHUD;
        A_Prompt += Prompt;
        A_Dialog += Dialog;
        A_ShowDialog += ShowDialog;
        A_HideDialog += HideDialog;
        

        PlayerAttributes.A_CoinChange += HandleCoinChange;
        PlayerAttributes.A_SoulChange += HandleSoulChange;

        fireColor1 = fireBar.color;
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
        UpdateShields();
    }

    void UpdateShields() {
        countShields.text = "Shields: "+ PlayerAttributes.shieldTokens.ToString();
    }


    void UpdateHealthBar() {

        healthText.text = $"{(int)PlayerAttributes.health}/ {(int)PlayerAttributes.maxHealth}"; 
        float newHP = PlayerAttributes.health / PlayerAttributes.maxHealth;
        // if (hp != newHP) StartCoroutine(AnimateFillAmountCoroutine(hp, newHP));
        if (hp != newHP) animationFill.DOFillAmount(newHP, 0.2f);
        fireBar.fillAmount = newHP;
        hp = newHP;


        // Low Health
        float lowHP = 0.5f;
        if (newHP < lowHP && !isLowHealth) {
            isLowHealth = true;
            StartBlinking();
            audioManager.Play("LowHealth");
        } 
        
        
        else if (newHP > lowHP && isLowHealth) {
            isLowHealth = false;
            StopBlinking();
            audioManager.Stop("LowHealth", fade: true);
        }
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



    void StartBlinking() {
        StopBlinking();
        blinkTween = fireBar.DOColor(fireColor2, t_blink).SetLoops(-1, LoopType.Yoyo);
    }

    void StopBlinking() {
        if (blinkTween != null && blinkTween.IsActive()) {
            blinkTween.Kill();
            blinkTween = null;
            
            fireBar.color = fireColor1;
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

    void ShowDialog() {
        dialogScreen.gameObject.SetActive(true);
    }

    void HideDialog() {
        dialogScreen.gameObject.SetActive(false);
    }


    void HandleSoulChange(int from, int to) {
        ShakeText(countSouls.transform);
    }

    void HandleCoinChange(int from,int to) {
        ShakeText(countCoins.transform);
    }

    void ShowHUD() {
        hudScreen.SetActive(true);
    }

    void HideHUD() {
        hudScreen.SetActive(false);
    }

    void Prompt(string text) {
        promptText.text = text;
    }


    void Dialog(string text) {
        ShowDialog();
        dialogScreen.DisplayText(text);
    }


    public void ShakeText(Transform tf) {

        float shakeDuration = 0.25f;
        float shakeStrength = 10f;
        int shakeVibrato = 50;
        tf.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);

        // float returnDuration = 0.25f;
        // tf.DOMove(originalPosition, returnDuration).SetDelay(returnDuration)
        //     .OnComplete(
        //         () => {goldCounter.color = originalGoldColor;}
        //     );
        }


}
