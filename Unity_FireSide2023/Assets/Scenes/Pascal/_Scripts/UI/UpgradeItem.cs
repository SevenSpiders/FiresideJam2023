using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Pascal {
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Upgrade Item", order = 1)]
    public class UpgradeItem : ScriptableObject
    {
        public Type type;
        public string title;
        public string explanation;
        public int baseCost;
        public Sprite sprite;
        public int cost => CalculateCost(PlayerAttributes.GetUpgradeLevel(type));

        int CalculateCost(int level) {
            List<int> numbers = new() {1, 2, 4, 10};
            int idx = Mathf.Min(numbers.Count-1, level);
            return baseCost * numbers[idx];
        }


        public enum Type {
            ArmorUpgrade = 0,
            FireUpgrade = 1,
            SoulUpgrade = 2,
            SpeedUpgrade = 3,
            DashUpgrade = 4,
        }
    }
}