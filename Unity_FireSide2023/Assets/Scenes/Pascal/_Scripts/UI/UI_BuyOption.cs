using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   
using UnityEngine.EventSystems;


namespace Pascal {

    public class UI_BuyOption : MonoBehaviour//, IPointerClickHandler
    {
        [SerializeField] UpgradeItem item;

        [SerializeField] TMP_Text title;
        [SerializeField] TMP_Text explanation;
        [SerializeField] TMP_Text cost;
        [SerializeField] Image img;


        void Start() {
            UpdateItem();
        }

        [ContextMenu("Update Item")]
        public void UpdateItem() {
            if (item == null) return;
            
            title.text = item.title;
            explanation.text = item.explanation;
            cost.text = item.cost.ToString();
            img.sprite = item.sprite;

        }

        // public void OnPointerClick(PointerEventData eventData) {
        //     HandleClick();
        // }


        public void HandleClick() {
            Debug.Log("click");
            UI_BuyScreen.A_ClickItem?.Invoke(item);
            UpdateItem();
        }

    }
}