using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
using TMPro;


namespace Pascal {
    public class SoulDepositZone : MonoBehaviour
    {
        
        [SerializeField] AudioManager audioManager;
        [SerializeField] Transform spawnPoint;
        [SerializeField] Transform offset;
        [SerializeField] Transform targetPoint;
        [SerializeField] Transform ghostPrefab;
        [SerializeField] TMP_Text collectionText;

        int totSoulsCollected;


        void OnTriggerEnter(Collider other) { 
            if (!other.CompareTag("Player")) return;
            int souls = PlayerAttributes.souls;

            if (souls == 0) return;


            for (int i=0; i< souls; i++) {
                // spawn
                float t_random = Random.Range(0f, 1f);
                Invoke(nameof(InstantiateGhost), t_random);
                // Transform ghost = Instantiate(ghostPrefab, transform);
                // float rng = Random.Range(-1f, 1f);
                // Vector3 dir = spawnPoint.position - offset.position;
                // ghost.position = spawnPoint.position + rng * dir;
                // ghost.DOMove(targetPoint.position, 2f);
            }

            // sell
            float coins = (float) PlayerAttributes.souls * PlayerAttributes.coinsPerSoul;
            PlayerAttributes.coins += (int) coins;
            PlayerAttributes.souls = 0;
            totSoulsCollected += souls;
            UpdateCollectionText();

            // scale vfx with souls
            audioManager.Play("Coins");
        }

        void InstantiateGhost() {
            Transform ghost = Instantiate(ghostPrefab, transform);
            float rng = Random.Range(-1f, 1f);
            Vector3 dir = spawnPoint.position - offset.position;
            ghost.position = spawnPoint.position + rng * dir;
            ghost.DOMove(targetPoint.position, 2f);
        }

        void UpdateCollectionText() {
            collectionText.text = totSoulsCollected.ToString();
        }

        
    }
}