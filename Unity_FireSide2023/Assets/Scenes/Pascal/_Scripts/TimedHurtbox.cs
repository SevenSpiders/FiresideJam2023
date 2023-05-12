using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal {

    public class TimedHurtbox : HurtBox
    {
        [SerializeField] float pauseTime;
        [SerializeField] float activeTime;
        [SerializeField] float offsetTime;
        public bool isActive;
        
        MeshRenderer mesh;

        float t2;

        void Awake() {
            t2 = -offsetTime;
            mesh = GetComponent<MeshRenderer>();
            mesh.enabled = isActive;
        }

        void Update() {
            t2 += Time.deltaTime;

            if (isActive && t2 >= activeTime) {
                Toggle(false);
            }

            if (!isActive && t2 >= pauseTime)
                Toggle(true);


        }

        void Toggle(bool b) {
            t2 = 0;
            isActive = b;
            mesh.enabled = b;
        }


        protected override void DealDamage(Collider other) {
            if (!isActive) return;
            base.DealDamage(other);
        }

    }

}
