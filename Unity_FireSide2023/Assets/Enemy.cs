using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int damageAmount = 25;
    private void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.GetComponent<PlayerAttributes>() != null)
        // {
        //     other.gameObject.GetComponent<PlayerAttributes>().Damage(damageAmount);
        // }
    }
}
