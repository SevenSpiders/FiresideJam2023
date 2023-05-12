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
        public bool isImmune;

        public int currentHealth;
        


        

        void Start() {
            currentHealth = maxHealth;
        }




        public void TakeDamage(int damageAmount)
        {
            if (isImmune) return;


            // Debug.Log($"{name} took damage {damageAmount}");
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
                Die();
            if (isPlayer) PlayerAttributes.health = currentHealth;
            audioManager.Play("Hurt");
            vfx.Play();
            isImmune = true;
            Invoke(nameof(EndImmunity), 0.5f);
        }

        public void EndImmunity() => isImmune = false;

        void Die()
        {
            Debug.Log("Player died!");
            A_Death?.Invoke(this.name);
        }
    }
}