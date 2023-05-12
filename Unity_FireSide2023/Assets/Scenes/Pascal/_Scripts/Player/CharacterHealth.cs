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
        [SerializeField] Material hurtMat;


        float t_immune = 0.5f;
        float t_blink = 0.2f;
        public bool isImmune;

        public int currentHealth;
        
        

        void Start() {
            currentHealth = maxHealth;
            EndBlink();
        }




        public void TakeDamage(int damageAmount)
        {
            if (isImmune) return;


            // Debug.Log($"{name} took damage {damageAmount}");
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
                Die();
            if (isPlayer) PlayerAttributes.health = currentHealth;
            if (audioManager != null) audioManager.Play("Hurt");
            if (hurtMat != null) hurtMat.SetFloat("_Hurt", 1f);
            if (vfx != null) vfx.Play();


            isImmune = true;
            Invoke(nameof(EndImmunity), t_immune);
            Invoke(nameof(EndBlink), t_blink);
        }

        public void EndImmunity() {
            isImmune = false;
            
        }

        void EndBlink() {
            if (isPlayer) hurtMat.SetFloat("_Hurt", 0);
        }

        void Die()
        {
            Debug.Log("Player died!");
            A_Death?.Invoke(this.name);
        }
    }
}