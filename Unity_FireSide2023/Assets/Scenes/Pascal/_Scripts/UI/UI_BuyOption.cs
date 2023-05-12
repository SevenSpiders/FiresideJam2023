using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   
using UnityEngine.EventSystems;


namespace Pascal {

    public class UI_BuyOption : MonoBehaviour, IPointerClickHandler
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

        public void OnPointerClick(PointerEventData eventData) {
            HandleClick();
        }


        public void HandleClick() {
            UI_BuyScreen.A_ClickItem?.Invoke(item);
        }

    }
}