using UnityEngine;
using UnityEngine.VFX;


namespace Pascal
{

    public class PlayerHealth : CharacterHealth
    {
        public static new System.Action<string> A_Death;    // string: characterName



        public override void TakeDamage(int damageAmount)
        {

            if (isImmune) return;

            float armor = PlayerAttributes.armor * 0.1f;
            if (PlayerAttributes.shieldActive) armor += 0.9f;

            int _dmg = (int)(damageAmount * Mathf.Max(1f - armor, 0));

            Debug.Log($"take damage: {damageAmount} {_dmg}, health {PlayerAttributes.health} / {PlayerAttributes.maxHealth}");

            base.TakeDamage(_dmg);
        }

        protected override void Die()
        {
            base.Die();
        }



    }
}