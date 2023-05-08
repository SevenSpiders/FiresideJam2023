using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAttributes
{
    // These values are the starting values in playmode
    // When we have a saving system then this values need to be overwritten from a json class or from unitys PlayerPref Class
    // Note that you cant see those for example movement values in the boat controller component cause they are stored here

    //Movement 
    public static float speed = 8f;
    public static float sprintSpeed = 12f;
    public static float accelRate = 10f;
    public static float deccelRate = 1.5f;
    public static float rotationRate = 1f;
    // Stats
    public static bool movementEnabled = true;
    public static float health = 100;
    public static float recovery = 25f;
    public static float regress = 5f;
    public static float maxHealth = 100;
    public static float coins = 0;
    public static float soulValueMultiplier = 2.5f;
    public static float souls = 0;

    public static bool isSafe = false;

    public static void AddCoins(int amount = 1) {
        if (souls > 0) {
            coins += Mathf.RoundToInt(amount * soulValueMultiplier);
            souls--;
        }
    }

    public static void Recover() {
        health = health < maxHealth ? health + (1 * recovery * Time.deltaTime) : maxHealth;
    }
    public static void Regress() {
        health = health >= 0 ? health - (1 * regress * Time.deltaTime) : 0;
    }
    public static void Heal(float amount) {
        health = health + Mathf.Abs(amount) <= maxHealth ? health + Mathf.Abs(amount) : maxHealth;
    }
    public static void Damage(float amount) {
        health = health - Mathf.Abs(amount) >= 0 ? health - Mathf.Abs(amount) : 0;
    }
}
