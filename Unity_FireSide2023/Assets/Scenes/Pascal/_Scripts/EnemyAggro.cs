using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pascal {



    public class EnemyAggro : MonoBehaviour
    {
        public System.Action<Transform> a_Trigger;

        [SerializeField] float aggroRadius = 10f;
        [SerializeField] LayerMask targetLayer;
        
        public bool hasTarget = false;
        Transform target;

        void Update() {

            if (hasTarget) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, aggroRadius, targetLayer);
            if (hits.Length > 0) {
                hasTarget = true;
                target = hits[0].transform;

                Debug.Log("Target acquired: " + target.name);
                a_Trigger?.Invoke(target);
            }
        }

        void OnDrawGizmosSelected() {
            // Draw aggro radius gizmo in editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
        }
    }
}