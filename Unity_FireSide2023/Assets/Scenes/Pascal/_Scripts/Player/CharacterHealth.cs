using UnityEngine;


namespace Pascal {

    public class CharacterHealth : MonoBehaviour
    {
        public static System.Action<string> A_Death;    // string: characterName

        
        [SerializeField] AudioManager audioManager;
        [SerializeField] Material hurtMat;


        float t_immune = 0.5f;
        float t_blink = 0.2f;
        public bool isImmune;

        
        

        void Start() {
            EndBlink();
        }




        public virtual void TakeDamage(int damageAmount)
        {
            if (isImmune) return;


            // Debug.Log($"{name} took damage {damageAmount}");
            
            PlayerAttributes.health -= damageAmount;
            if (PlayerAttributes.health <= 0) Die();
            if (audioManager != null) audioManager.Play("Hurt");
            if (hurtMat != null) hurtMat.SetFloat("_Hurt", 1f);


            isImmune = true;
            Invoke(nameof(EndImmunity), t_immune);
            Invoke(nameof(EndBlink), t_blink);
        }

        public void EndImmunity() {
            isImmune = false;
            
        }

        void EndBlink() {
            if(hurtMat != null) hurtMat.SetFloat("_Hurt", 0);
        }

        protected virtual void Die()
        {
            Debug.Log("Player died!");
            A_Death?.Invoke(this.name);
        }
    }
}