using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {

    public class CharacterHealth : MonoBehaviour
    {
        public static System.Action<string> A_Death;    // string: characterName

        
        [SerializeField] int maxHealth = 100;
        [SerializeField] bool isPlayer;
        [SerializeField] AudioManager audioManager;
        [SerializeField] VisualEffect vfx;

        public int currentHealth;


        

        void Start() {
            currentHealth = maxHealth;
        }




        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
                Die();
            if (isPlayer) PlayerAttributes.health = currentHealth;
            audioManager.Play("Hurt");
            vfx.Play();
        }

        void Die()
        {
            // Game over logic goes here
            Debug.Log("Player died!");
            A_Death?.Invoke(this.name);
        }
    }
}