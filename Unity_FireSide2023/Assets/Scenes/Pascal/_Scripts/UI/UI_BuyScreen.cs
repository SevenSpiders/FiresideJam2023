using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Pascal {


    public class UI_BuyScreen : MonoBehaviour
    {

        public static System.Action<UpgradeItem> A_ClickItem;

        [SerializeField] TMP_Text goldCounter;
        [SerializeField] List<UI_BuyOption> options;
        [SerializeField] List<UpgradeItem> upgrades;
        [SerializeField] AudioManager audioManager;
        

        void Awake() {
            A_ClickItem += HandleOptionClick;
        }


        public void HandleOptionClick(UpgradeItem item) {
            Debug.Log($"buy item? {item.type}");

            if (item.cost > PlayerAttributes.coins) {
                Debug.LogWarning("not enough coins");
                audioManager.Play("NoBuy");
                return;
            }

            PlayerAttributes.coins -= item.cost;
            switch (item.type) {

                case UpgradeItem.Type.ArmorUpgrade:
                    PlayerAttributes.armorUpgrades += 1;
                    break;
                
                case UpgradeItem.Type.SpeedUpgrade:
                    PlayerAttributes.speedUpgrades += 1;
                    break;
                
                case UpgradeItem.Type.SoulUpgrade:
                    PlayerAttributes.soulUpgrades += 1;
                    break;
                
                case UpgradeItem.Type.FireUpgrade:
                    PlayerAttributes.fireUpgrades += 1;
                    break;
                
                default:
                    Debug.LogWarning($"item type not implemented: {item.type}");
                    break;
            }

            audioManager.Play("Buy");
            
        }

        [ContextMenu("update items")]
        void UpdateItems() {
            options.ForEach(o => o.UpdateItem());
        }
    }

    
}