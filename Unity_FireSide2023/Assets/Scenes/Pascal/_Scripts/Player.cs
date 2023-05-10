using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal {

    public class Player : MonoBehaviour
    {

        [SerializeField] AudioManager audioManager;

        void Start() {
            CharacterHealth.A_Death += HandleDeath;

        }

        void HandleDeath(string charName) {
            if (charName != this.name) return;
            Debug.Log($"{this.name} died");
        }
    }
}