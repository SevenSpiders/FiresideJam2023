using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal
{


    public class CollectableFire : Collectable
    {

        
        [SerializeField] AudioManager audioManager;
        [SerializeField] ParticleSystem vfx;

        protected override void OnCollect(){
            audioManager.Play("Collect");
            if (vfx != null) vfx.Play();
            Invoke(nameof(Vanish), 1f);
            audioManager.Play("Trigger");
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, collectionRadius);
        }

        void Vanish() {
            Destroy(gameObject);
        }
    }
}

