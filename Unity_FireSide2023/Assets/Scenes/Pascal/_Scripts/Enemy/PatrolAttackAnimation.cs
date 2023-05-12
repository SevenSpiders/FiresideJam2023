using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



namespace Pascal {

    public class PatrolAttackAnimation : MonoBehaviour
    {

        [SerializeField] HurtBox hurtBox;
        [SerializeField] float moveDuration = 0.5f;
        [SerializeField] Ease ease;

        Vector3 initialPosition;
        bool isOnCooldown;
        float t_cooldown = 1f;

        void Start()
        {
            hurtBox.a_Trigger += HandleTrigger;
            initialPosition = transform.localPosition;
        }

        
        void HandleTrigger(Collider target) {
            if (isOnCooldown) return;
            isOnCooldown = true;
            transform.LookAt(target.transform);
            transform.DOMove(target.transform.position, moveDuration).SetEase(ease)
                .OnComplete(() =>
                {
                    transform.DOLocalMove(initialPosition, moveDuration).SetEase(ease);
                    transform.localEulerAngles = Vector3.zero;
                });
            Invoke(nameof(EndCooldown), t_cooldown);
        }

        void EndCooldown() => isOnCooldown = false;
    }

}
