using UnityEngine;
using UnityEngine.VFX;


namespace Pascal
{

    public class PlayerHealth : CharacterHealth
    {
        public static new System.Action<string> A_Death;    // string: characterName

        [SerializeField] AudioManager audioManager;
        [SerializeField] VisualEffect vfx;
        [SerializeField] Material hurtMat;











        public override void TakeDamage(int damageAmount)
        {
            if (isImmune) return;

            PlayerAttributes.health = currentHealth;
            float armor = PlayerAttributes.armorUpgrades * 0.1f;
            if (PlayerAttributes.shieldActive) armor += 0.9f;

            damageAmount = (int)(damageAmount * Mathf.Max(1f - armor, 0));

            base.TakeDamage(damageAmount);
        }

        protected override void Die()
        {
            base.Die();
        }



    }
}