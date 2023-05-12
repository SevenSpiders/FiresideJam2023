using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal {

    public class TimedHurtbox : HurtBox
    {
        [SerializeField] float pauseTime;
        [SerializeField] float activeTime;
        [SerializeField] float offsetTime;

        [SerializeField] Material activeMat;
        [SerializeField] Material inactiveMat;
        [SerializeField] Material blickMat;

        
        
        public bool isActive;
        
        MeshRenderer mesh;

        float t_warn = 0.6f;
        float t2;

        void Awake() {
            t2 = -offsetTime;
            mesh = GetComponent<MeshRenderer>();
            // CopyMaterial();
            // mesh.enabled = isActive;
        }

        void Update() {
            t2 += Time.deltaTime;

            if (isActive && t2 >= activeTime) {
                ToggleActive(false);
            }

            if (!isActive && t2 >= pauseTime)
                ToggleActive(true);
            
            if (!isActive && t_warn > pauseTime - t2 )
                StartBlinking();

        }


        // void CopyMaterial() {
        //     Material originalMaterial = mesh.material;
        //     hurtMat = Instantiate(originalMaterial);
        //     mesh.material = hurtMat;
        // }



        void StartBlinking() {
            mesh.material = blickMat;
        }


        void ToggleActive(bool b) {
            t2 = 0;
            isActive = b;
            // mesh.enabled = b;
            int i = b ? 0 : 1;
            mesh.material = b ? activeMat : inactiveMat;
        }


        protected override void DealDamage(Collider other) {
            if (!isActive) return;
            base.DealDamage(other);
        }

    }

}
