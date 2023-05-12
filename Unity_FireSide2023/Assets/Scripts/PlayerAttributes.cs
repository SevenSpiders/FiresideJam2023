using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAttributes
{
    // These values are the starting values in playmode
    // When we have a saving system then this values need to be overwritten from a json class or from unitys PlayerPref Class
    // Note that you cant see those for example movement values in the boat controller component cause they are stored here

    //Movement 
    public static float speed = 15f;
    public static float sprintSpeed = 18f;
    public static float boostSpeed = 0;
    public static float boostCooldown = 0;
    public static float accelRate = 10f;
    public static float deccelRate = 1.5f;
    public static float rotationRate = 2f;
    // Stats
    public static bool movementEnabled = true;
    public static float health = 100;
    public static float recovery = 25f;
    public static float regress = 5f;
    public static float maxHealth = 100;
    public static float coins = 0;
    public static float soulValueMultiplier = 2.5f;
    public static int souls = 0;
    public static int soulsMax = 1;
    // Music Settings
    public static float musicFadeTime = 8f;
    public static float musicMaxVolume = .01f;
    public static float atmosphereVolume = .4f;

    public static bool isSafe = false;
    public static bool isAttacked = false;
    public static GameObject curCurrent = null;


    // Upgrades
    public static int speedUpgrades;
    public static int armorUpgrades;
    public static int fireUpgrades;
    public static int soulUpgrades;



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
    public static void Heal(float amount) {
        health = health + Mathf.Abs(amount) <= maxHealth ? health + Mathf.Abs(amount) : maxHealth;
    }
    public static void Damage(float amount) {
        health = health - Mathf.Abs(amount) >= 0 ? health - Mathf.Abs(amount) : 0;
    }
    public static void DamageOverTime(float amount) {
        health = health >= 0 ? health - (amount * Time.deltaTime) : 0;
    }
}
