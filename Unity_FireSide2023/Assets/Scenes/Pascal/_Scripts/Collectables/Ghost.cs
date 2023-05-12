using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


namespace Pascal
{


    public class Ghost : Collectable
    {

        
        [SerializeField] AudioManager audioManager;
        [SerializeField] MeshRenderer ghostModel;
        [SerializeField] VisualEffect vfx;
        [SerializeField] float t_cooldown = 100f;


        protected override void OnCollect(){
            audioManager.Play("Collect");
            if (vfx != null) vfx.Play();
            ghostModel.enabled = false;
            Invoke(nameof(Reset), t_cooldown);
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, collectionRadius);
        }

        void Reset() {
            ghostModel.enabled = true;
            hasBeenCollected = false;
        }
    }
}

