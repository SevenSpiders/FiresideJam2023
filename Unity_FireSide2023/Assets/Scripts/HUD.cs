using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class HUD : MonoBehaviour
{

    public static System.Action A_ShowBuyScreen;
    public static System.Action A_HideBuyScreen;
    public static System.Action A_ShowGameOver;
    public static System.Action A_HideGameOver;



    public TextMeshProUGUI coinsUI;


    [SerializeField] TMP_Text speedUI;
    [SerializeField] Image fireBar;
    [SerializeField] List<Image> soulOrbs;
    [SerializeField] Pascal.UI_BuyScreen buyScreen;


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

        coinsUI.text = "Coins: " + Mathf.RoundToInt(PlayerAttributes.coins).ToString();
        fireBar.fillAmount = PlayerAttributes.health / PlayerAttributes.maxHealth;
        UpdateSouls();
    }


    void UpdateSouls() {
        int maxSouls = PlayerAttributes.soulsMax;
        int souls = PlayerAttributes.souls;
        for (int i=0; i< soulOrbs.Count; i++) {
            soulOrbs[i].gameObject.SetActive(i< maxSouls );
            soulOrbs[i].color = (i < souls) ? Color.white : Color.grey;
        }
    }



    void OpenBuyScreen() => buyScreen.gameObject.SetActive(true);
    void CloseBuyScreen() => buyScreen.gameObject.SetActive(false);
    void ShowGameOver() {
        //
    }

    void HideGameOver() {

    }

}
