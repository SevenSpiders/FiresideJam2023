using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal
{


    public class CollectableFire : Collectable
    {

        
        [SerializeField] AudioManager audioManager;
        [SerializeField] VisualEffect vfx;


        protected override void OnCollect(){
            audioManager.Play("Collect");
            vfx.Play();
            Invoke(nameof(Vanish), 1f);
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

