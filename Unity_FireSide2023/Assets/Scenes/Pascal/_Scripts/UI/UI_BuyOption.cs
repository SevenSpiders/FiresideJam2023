using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   


namespace Pascal {

    public class UI_BuyOption : MonoBehaviour
    {
        [SerializeField] UpgradeItem item;

        [SerializeField] TMP_Text title;
        [SerializeField] TMP_Text cost;
        [SerializeField] Image img;


        void Start() {
            UpdateItem();
        }

        [ContextMenu("Update Item")]
        public void UpdateItem() {
            if (item == null) return;
            
            title.text = item.name;
            cost.text = item.cost.ToString();
            img.sprite = item.sprite;

        }


        public void HandleClick() {
            Debug.LogWarning("click");
            UI_BuyScreen.A_ClickItem?.Invoke(item);
        }

    }
}