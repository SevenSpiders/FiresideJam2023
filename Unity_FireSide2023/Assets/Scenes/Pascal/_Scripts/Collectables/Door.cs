using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Pascal {

    public class Door : MonoBehaviour
    {
        [SerializeField] AreaTrigger triggerOpen;
        [SerializeField] AreaTrigger triggerClose;

        [SerializeField] Vector3 offset;
        [SerializeField] float duration;

        Vector3 posInit;

        void Awake() {
            if (triggerOpen != null) triggerOpen.a_Trigger += Open;
            if (triggerClose != null) triggerClose.a_Trigger += Close;
            posInit = transform.position;
        }


        void Open() {
            transform.DOMove(posInit + offset, duration);
        }

        void Close() {
            transform.DOMove(posInit, duration);
        }
    }

}
