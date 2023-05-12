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
            Debug.Log($"buy item? {item.name}");
            // check if gold
            // buy
        }

        [ContextMenu("update items")]
        void UpdateItems() {
            options.ForEach(o => o.UpdateItem());
        }
    }

    
}