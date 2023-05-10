using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal {

    public class Collectable : MonoBehaviour
    {

        public static System.Action<Type, float> A_Collect;

        public enum Type {
            Soul = 0,
            Coin = 1,
            Fire =2,
        }

        [SerializeField] protected float collectionRadius = 10f;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected Type type;
        [SerializeField] protected float value;

        protected bool hasBeenCollected;


        void Update() {
            if(hasBeenCollected) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, collectionRadius, targetLayer);
            if (hits.Length > 0) {
                hasBeenCollected = true;
                A_Collect?.Invoke(type, value);
                OnCollect();
                
            }
        }

        protected virtual void OnCollect() {

        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, collectionRadius);
        }

    }










}

