using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Pascal {

    [SelectionBase]
    public class Player : MonoBehaviour
    {
        
        [SerializeField] AudioManager audioManager;
        [SerializeField] float healthDecayPerSecond = 0.5f;
        [SerializeField] Transform respawnPoint;
        [SerializeField] float t_tick = 2f;
        bool isDead;

        float t;

        void Start() {
            CharacterHealth.A_Death += HandleDeath;
            Collectable.A_Collect += HandleCollect;

        }

        

        void HandleCollect(Collectable.Type _type, float value) {

            switch (_type) {
                case Collectable.Type.Soul:
                    PlayerAttributes.souls += (int)value;
                    break;
                case Collectable.Type.Fire:
                    PlayerAttributes.Heal(value);
                    break;
                case Collectable.Type.Coin:
                    PlayerAttributes.coins += (int)value;
                    break;
                default: break;
            }
                
        }

        void Update() {
            t += Time.deltaTime;
            if (t > t_tick) {
                PlayerAttributes.health -= healthDecayPerSecond*t_tick;
                if (PlayerAttributes.health <= 0) 
                    HandleDeath(this.name);
                t = 0;
            }
        }


        void HandleDeath(string charName) {
            if (charName != this.name) return;
            Debug.Log($"{this.name} died");
            Vector3 pos = transform.position + Vector3.down * 30f;
            isDead = true; // playerattributes
            HUD.A_ShowGameOver?.Invoke();
            audioManager.Play("Death");
            transform.DOMove(pos, 5f).OnComplete(
                ()=> {Respawn();}
            );

            PlayerAttributes.souls = 0;
            PlayerAttributes.coins /= 2;
        }


        void Respawn() {
            HUD.A_HideGameOver?.Invoke();
            transform.position= respawnPoint.position;
            PlayerAttributes.health = PlayerAttributes.maxHealth;
            isDead = false;
        }
        
    }
}