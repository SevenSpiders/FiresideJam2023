using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Pascal
{


    public class Ghost : Collectable
    {

        
        [SerializeField] AudioManager audioManager;
        [SerializeField] MeshRenderer ghostModel;
        [SerializeField] float t_cooldown = 100f;
        [SerializeField] SpriteRenderer ring1;
        [SerializeField] SpriteRenderer ring2;
        [SerializeField] float t_ring = 0.3f;
        [SerializeField] ParticleSystem particles;
        // [SerializeField] Ease ease;

        Ease ease = Ease.OutQuad;

        Vector3 scaleInit = Vector3.one;

        void Start() {
            scaleInit = ring2.transform.localScale;
        }

        protected override void OnCollect(){
            ring1.DOFade(0.2f, 0.3f);
            audioManager.Play("Collect");
            RingAnimation();
            ghostModel.enabled = false;
            Invoke(nameof(Reset), t_cooldown);
            if (particles != null && !particles.isPlaying)
                particles.Play();

        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, collectionRadius);
        }

        void Reset() {
            ring1.DOFade(1f, 0.3f);
            ghostModel.enabled = true;
            hasBeenCollected = false;
        }


        void RingAnimation() {
            ring2.enabled = true;
            ring2.transform.DOScale(10f, t_ring).SetEase(ease);        
            ring2.DOFade(0, t_ring+0.01f)
                .SetEase(ease)
                .OnComplete(
                    () => ResetRing()
                );
        }

        void ResetRing() {
            ring2.enabled = false;
            ring2.transform.DOScale(scaleInit, 0);
            ring2.DOFade(1f, 0);
        }
    }
}

