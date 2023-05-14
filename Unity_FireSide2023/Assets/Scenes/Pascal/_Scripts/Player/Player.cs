using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Pascal {

    [SelectionBase]
    public class Player : MonoBehaviour
    {

        [Header("Start attributes")]
        [SerializeField] float _health;
        [SerializeField] int boostTokens;
        [SerializeField] int shieldTokens;
        [SerializeField] float t_boostRecover = 5f;
        // [SerializeField] float _speed;
        // [SerializeField] float 

        [SerializeField] Camera playerCam;
        [SerializeField] AudioManager audioManager;
        [SerializeField] Transform respawnPoint;

        [SerializeField] float healthDecayPerSecond = 0.5f;
        [SerializeField] float t_tick = 2f;
        float t_safeZoneCooldown = 1f;
        float t_deathAnimation = 5f;


        [SerializeField] GameObject meshObj;
        
        


        // private
        Vector3 meshPosInit = Vector3.zero;
        bool isDead;
        float t;







        // ------------- START -------------

        void Start() {
            PlayerAttributes.maxHealth = _health;
            PlayerAttributes.health = _health;
            PlayerAttributes.boostTokens = boostTokens;
            PlayerAttributes.boostTokensMax = boostTokens;
            PlayerAttributes.shieldTokens = shieldTokens;
            PlayerAttributes.shieldLevel = shieldTokens;

            CharacterHealth.A_Death += HandleDeath;
            Collectable.A_Collect += HandleCollect;
            meshPosInit = meshObj.transform.localPosition;
            Invoke(nameof(RecoverBoostToken), t_boostRecover);
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









        // ---------- UPDATE ------------

        void Update() {
            t += Time.deltaTime;

            // SAFE ZONE
            if (PlayerAttributes.isInSafeZone) {
                if (t > t_safeZoneCooldown)
                    SetSafeZone(false);
            }

            // DECAY
            else {
                if (t > t_tick && !PlayerAttributes.isDead && !PlayerAttributes.isImmune) {
                    PlayerAttributes.health -= healthDecayPerSecond*t_tick;
                    audioManager.Play("Decay");
                    if (PlayerAttributes.health <= 0) 
                        HandleDeath(this.name);
                    t = 0;
                }
            }

            
        }



        // Check
        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("SafeZone")) return;
            SetSafeZone(true);
        }

        void OnTriggerStay(Collider other) {
            if (!other.CompareTag("SafeZone")) return;
            SetSafeZone(true);
        }

        void OnTriggerExit(Collider other) { 
            if (!other.CompareTag("SafeZone")) return;
            SetSafeZone(false);
        }

        void SetSafeZone(bool b) {
            PlayerAttributes.isInSafeZone = b;
            t = 0;
        }




        void RecoverBoostToken() {
            PlayerAttributes.boostTokens += 1;
            Invoke(nameof(RecoverBoostToken), t_boostRecover);
        }




        // ------------- DEATH ---------------

        void HandleDeath(string charName) {
            if (charName != this.name) return;
            Debug.LogWarning($"you died");
            

            playerCam.DOFieldOfView(30, t_deathAnimation).SetEase(Ease.OutCubic);

            PlayerAttributes.isDead = true;
            Vector3 sunkenPos = Vector3.down * 30f;
            
            audioManager.Play("Death");
            meshObj.transform.DOLocalMove(sunkenPos, t_deathAnimation)
                .SetEase(Ease.OutCubic)
                .OnComplete( ()=> {Respawn();} );

            PlayerAttributes.soulLoss = PlayerAttributes.souls;
            PlayerAttributes.souls = 0;
            int coinLoss = PlayerAttributes.coins /= 2;
            PlayerAttributes.coinLoss = coinLoss;
            PlayerAttributes.coins -= coinLoss;

            HUD.A_ShowGameOver?.Invoke();
        }


        void Respawn() {
            HUD.A_HideGameOver?.Invoke();
            transform.position= respawnPoint.position;
            meshObj.transform.localPosition = meshPosInit;
            PlayerAttributes.health = PlayerAttributes.maxHealth;
            PlayerAttributes.isDead = false;
            playerCam.fieldOfView = 60f;
        }
        
    }
}