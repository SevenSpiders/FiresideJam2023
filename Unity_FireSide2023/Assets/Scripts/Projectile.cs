using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;

    private void Awake() {
        StartCoroutine(DestroyProjectile());    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyPatrol>() == null)
            return;

        Destroy(other.gameObject);
    }

    private IEnumerator DestroyProjectile() {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}
