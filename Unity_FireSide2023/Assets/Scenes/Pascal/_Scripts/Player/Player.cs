using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal {

    public class Player : MonoBehaviour
    {
        
        [SerializeField] AudioManager audioManager;

        void Start() {
            CharacterHealth.A_Death += HandleDeath;
            Collectable.A_Collect += HandleCollect;

        }

        void HandleDeath(string charName) {
            if (charName != this.name) return;
            Debug.Log($"{this.name} died");
        }

        void HandleCollect(Collectable.Type _type, float value) {

            switch (_type) {
                case Collectable.Type.Soul:
                    PlayerAttributes.souls += value;
                    break;
                case Collectable.Type.Fire:
                    PlayerAttributes.health += value;
                    break;
                case Collectable.Type.Coin:
                    PlayerAttributes.coins += value;
                    break;
                default: break;
            }
                
        }
    }
}