using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Pascal
{
    public class UI_Dialog : MonoBehaviour
    {
        [SerializeField] Image portraitA;
        [SerializeField] Image portraitB;
        [SerializeField] TMP_Text dialog;


        float typingSpeed = 0.01f;

        public void DisplayText(string text) {
            StartCoroutine(TypeText(text));
        }

        IEnumerator TypeText(string displayText) {

            dialog.text = string.Empty;

            for (int i = 0; i < displayText.Length; i++)
            {
                dialog.text += displayText[i];
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}
