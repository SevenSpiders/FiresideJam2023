using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pascal;


public static class PlayerAttributes
{
    
    public static System.Action<int, int>    A_CoinChange; // from to
    public static System.Action<int, int>    A_SoulChange;  // from to
    public static System.Action<float, float> A_HealthChange;

    //Movement 
    public static bool inputsDisabled; // for intro
    static float _speed = 15f;
    public static float speed {
        get => _speed;
        set =>_speed = value;
    }
    public static float boostSpeed = 0;
    static int _boostTokens = 0;
    public static int boostTokens {
        get => _boostTokens;
        set => _boostTokens = Mathf.Min(value, boostTokensMax);
    }

    static int _boostTokensMax = 3;
    public static int boostTokensMax {
        get =>  _boostTokensMax + dashLevel;
        set => _boostTokensMax = value;
    }



    // HEALTH
    static float _health = 30;
    public static float health {
        get => _health;
        set {
            if (isImmune) return;
            value = Mathf.Min(value, maxHealth);
            A_HealthChange?.Invoke(_health, value);
            _health = value;
        }
    }
    static float _maxHealth = 50;
    public static float maxHealth {
        get => _maxHealth + fireLevel*10f;
        set => _maxHealth = value;
    }
    public static float HP => health/ maxHealth;
    public static void Heal(float amount) {
        health = health + Mathf.Abs(amount) <= maxHealth ? health + Mathf.Abs(amount) : maxHealth;
    }
    public static float armor => armorLevel * 0.1f;
    public static bool isDead = false;
    public static bool isImmune;



    // SOULS
    static int _souls = 0;
    public static int souls {
        get => _souls;
        set {
            A_SoulChange?.Invoke(_souls, value);
            _souls = value;
        }
    }
    public static int soulLoss = 0;
    public static int soulsMax = 10000;


    // GOLD
    static int _coins = 0;
    public static int coins {
        get => _coins;
        set {
            A_CoinChange?.Invoke(_coins, value);
            _coins = value;
        }
    }
    public static int coinLoss = 0;
    public static float coinsPerSoul = 1f;

    public static bool shieldActive = false;
    public static bool isInSafeZone = false;



    // UPGRADES
    public static int maxLevel = 3;
    public static int speedLevel;
    public static int armorLevel;
    public static int fireLevel;
    public static int soulLevel;
    public static int dashLevel;


    public static int GetUpgradeLevel(UpgradeItem.Type type) {
        switch (type) {
            case UpgradeItem.Type.FireUpgrade: return fireLevel;
            case UpgradeItem.Type.SpeedUpgrade: return speedLevel;
            case UpgradeItem.Type.DashUpgrade: return dashLevel;
            default: return 0;
        }
    }









































    // DPERECATED
    public static float sprintSpeed = 18f;
    
    public static float boostCooldown = 0;
    public static float accelRate = 10f;
    public static float deccelRate = 1.5f;
    public static float rotationRate = 2f;
    // Stats
    public static bool movementEnabled = true;
    
    public static float recovery = 25f;
    public static float regress = 5f;
    
    
    public static float soulValueMultiplier = 2.5f;
    
    // Music Settings
    public static float musicFadeTime = 8f;
    public static float musicMaxVolume = .01f;
    public static float atmosphereVolume = .4f;

    public static bool isSafe = false;
    public static bool isAttacked = false;
    public static GameObject curCurrent = null;

    


    



    public static void AddCoins(int amount = 1) {
        if (souls > 0) {
            coins += Mathf.RoundToInt(amount * soulValueMultiplier);
            souls--;
        }
    }

    public static void Recover() {
        health = health < maxHealth ? health + (recovery * Time.deltaTime) : maxHealth;
    }
    public static void Regress() {
        health = health >= 0 ? health - (regress * Time.deltaTime) : 0;
    }
    
    public static void Damage(float amount) {
        health = health - Mathf.Abs(amount) >= 0 ? health - Mathf.Abs(amount) : 0;
    }
    public static void DamageOverTime(float amount) {
        health = health >= 0 ? health - (amount * Time.deltaTime) : 0;
    }
}
