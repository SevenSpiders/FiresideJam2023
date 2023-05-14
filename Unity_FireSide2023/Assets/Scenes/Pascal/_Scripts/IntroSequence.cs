using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Pascal {
    public class IntroSequence : MonoBehaviour
    {
        /*
        display prompt: move with wasd
        charon is waiting for you

        display dialog ui

        waitfor input
        disable blocks
        start game



        */
        [System.Serializable]
        public struct DialogLines {
            [TextAreaAttribute] public string text;
        }


        [SerializeField] List<GameObject> boundries;
        [SerializeField] AreaTrigger charon;
        [SerializeField] Transform charonTarget; // his vacation

        [SerializeField] List<DialogLines> dialogLines;

        

        int textIdx;
        public bool waitForInput;
        float t_wait = 2f;



        void Start() {
            charon.a_Trigger += HandleCharonReached;
            SetBoundries(true);
            HUD.A_Prompt("WASD to Move");
            PlayerAttributes.isImmune = true;
        }

        void SetBoundries(bool b) {
            for (int i=0; i<boundries.Count; i++) {
                boundries[i].SetActive(b);
            }
        }

        string GetTextLine() => textIdx < dialogLines.Count ? dialogLines[textIdx].text : "...";

        void HandleCharonReached() {
            HUD.A_Prompt("");
            HUD.A_HideHUD();
            HUD.A_Dialog(GetTextLine());
            PlayerAttributes.inputsDisabled = true;
            Invoke(nameof(ProgressTextIdx), t_wait);
        }

        void ShowNextLine() {
            if (textIdx >= dialogLines.Count) {
                Reset();
                VanishCharon();
                return;
            }
            HUD.A_Dialog(GetTextLine());
            Invoke(nameof(ProgressTextIdx), t_wait);
        }

        void ProgressTextIdx() {
            waitForInput = true;
            textIdx += 1;
        }


        void Update() {
            if (Input.anyKeyDown && waitForInput) {
                waitForInput = false;
                ShowNextLine();
            }
        }

        void VanishCharon() {
            charon.transform.DOMove(charonTarget.position, 2f)
                .OnComplete(
                    () => charon.gameObject.SetActive(false)
                );
        }

        void Reset() {
            HUD.A_ShowHUD();
            HUD.A_HideDialog();
            PlayerAttributes.isImmune = false;
            PlayerAttributes.inputsDisabled = false;
            SetBoundries(false);
        }
    }
}
