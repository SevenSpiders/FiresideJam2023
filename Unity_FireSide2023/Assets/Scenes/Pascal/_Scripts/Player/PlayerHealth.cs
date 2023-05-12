using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {

    public class PlayerHealth : CharacterHealth
    {
        public static System.Action<string> A_Death;    // string: characterName

        [SerializeField] AudioManager audioManager;
        [SerializeField] VisualEffect vfx;
        [SerializeField] Material hurtMat;


        
        
        

        




        public override void TakeDamage(int damageAmount)
        {
            if (isImmune) return;

            PlayerAttributes.health = currentHealth;
            
            base.TakeDamage(damageAmount);
        }

        protected override void Die() {
            base.Die();
        }


        
    }
}