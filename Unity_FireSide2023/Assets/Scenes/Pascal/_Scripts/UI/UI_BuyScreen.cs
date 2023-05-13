using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Pascal {


    public class UI_BuyScreen : MonoBehaviour
    {

        public static System.Action<UpgradeItem> A_ClickItem;

        [SerializeField] TMP_Text goldCounter;
        [SerializeField] List<UI_BuyOption> options;
        [SerializeField] List<UpgradeItem> upgrades;
        [SerializeField] AudioManager audioManager;

        Color originalGoldColor;
        Vector3 originalPosition;

        void Awake() {
            A_ClickItem += HandleOptionClick;
            originalGoldColor = goldCounter.color;
            originalPosition = goldCounter.transform.position;
        }


        public void HandleOptionClick(UpgradeItem item) {
            Debug.Log($"buy item? {item.type}");

            if (item.cost > PlayerAttributes.coins) {
                Debug.LogWarning("not enough coins");
                audioManager.Play("NoBuy");
                ShakeText(Color.red);
                return;
            }

            PlayerAttributes.coins -= item.cost;
            switch (item.type) {

                case UpgradeItem.Type.ArmorUpgrade:
                    PlayerAttributes.armorLevel += 1;
                    break;
                
                case UpgradeItem.Type.SpeedUpgrade:
                    PlayerAttributes.speedLevel += 1;
                    break;
                
                case UpgradeItem.Type.DashUpgrade:
                    PlayerAttributes.dashLevel += 1;
                    break;
                
                case UpgradeItem.Type.FireUpgrade:
                    PlayerAttributes.fireLevel += 1;
                    break;
                
                default:
                    Debug.LogWarning($"item type not implemented: {item.type}");
                    break;
            }

            audioManager.Play("Buy");
            ShakeText(originalGoldColor);
            goldCounter.text = PlayerAttributes.coins.ToString();
            
        }

        void OnEnable() {
            UpdateItems();
            goldCounter.text = PlayerAttributes.coins.ToString();
        }


        public void ShakeText(Color _c) {

            goldCounter.color = _c;

            // Shake the text for a short duration
            float shakeDuration = 0.25f;
            float shakeStrength = 10f;
            int shakeVibrato = 50;
            goldCounter.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);

            // Return the text color and position to their original values
            float returnDuration = 0.25f;
            // goldCounter.DOColor(originalGoldColor, returnDuration);
            goldCounter.transform.DOMove(originalPosition, returnDuration).SetDelay(returnDuration)
                .OnComplete(
                    () => {goldCounter.color = originalGoldColor;}
                );
        }


        [ContextMenu("update items")]
        void UpdateItems() {
            options.ForEach(o => o.UpdateItem());
        }


        public void Close() => gameObject.SetActive(false);
    }

    
}