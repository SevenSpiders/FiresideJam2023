using UnityEngine;
using UnityEngine.VFX;


namespace Pascal {

    public class PlayerHealth : CharacterHealth
    {




        public override void TakeDamage(int damageAmount)
        {
            if (isImmune) return;
            if (PlayerAttributes.isDead) return;

            PlayerAttributes.health = currentHealth;
            float armor = PlayerAttributes.armor * 0.1f;
            if (PlayerAttributes.shieldActive) armor += 0.9f;

            damageAmount = (int) (damageAmount * Mathf.Max(1f- armor, 0));
            
            base.TakeDamage(damageAmount);
        }

        protected override void Die() {
            base.Die();
        }


        
    }
}