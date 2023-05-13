using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Pascal {
    public class UI_Icon : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Image background;

        public void SetAvailable(bool b) {
            Show();
            icon.enabled = b;
        }


        public void Show() => ToggleVisible(true);
        public void Hide() => ToggleVisible(false);

        void ToggleVisible(bool b) {
            icon.enabled = b;
            background.enabled =b;
        }
    }   
}
