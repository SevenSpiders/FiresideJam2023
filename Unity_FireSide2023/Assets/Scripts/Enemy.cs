using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int damageAmount = 25;
    private void OnCollisionEnter(Collision other)
    {
        PlayerAttributes.Damage(damageAmount);
    }
}
