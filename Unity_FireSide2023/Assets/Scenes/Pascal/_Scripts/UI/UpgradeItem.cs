using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Pascal {
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Upgrade Item", order = 1)]
    public class UpgradeItem : ScriptableObject
    {
        public string name;
        public int cost;
        public Type type;
        public Sprite sprite;

        public enum Type {
            ArmorUpgrade = 0,
            FireUpgrade = 1,
            SoulUpgrade = 2,
            SpeedUpgrade = 3,
        }
    }
}