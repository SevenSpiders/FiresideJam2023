using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedBoostOrb : MonoBehaviour
{
    public float speedBoostAmount;
    public float speedBoostTime;
    public float effectRadius;
    public float coolDown;
    public bool deleteAfterUse = false;

    private SphereCollider col;
    private bool isCoolingDown = false;

    private void Awake() {
        col = AddSphereCollider();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }

    private SphereCollider AddSphereCollider() {
        // Get existing collider or create one and apply values
        SphereCollider col = GetComponent<SphereCollider>() != null ? GetComponent<SphereCollider>() : this.gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = effectRadius;

        return col;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!isCoolingDown)
            StartCoroutine(StartBoost());
    }

    private IEnumerator StartBoost()
    {
        isCoolingDown = true;

        PlayerAttributes.boostCooldown = coolDown;
        float t = 0;

        while ( t < 1f) {
            t += Time.deltaTime;
            PlayerAttributes.boostSpeed = Mathf.Lerp(PlayerAttributes.boostSpeed, speedBoostAmount, t);
            yield return null;
        }

        if (deleteAfterUse)
            Destroy(this.gameObject);
        yield return new WaitForSeconds(coolDown);
        isCoolingDown = false;

    }



}
