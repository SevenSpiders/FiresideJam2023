using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;



public class SpeedBoostOrb : MonoBehaviour
{
    public float speedBoostAmount;
    public float speedBoostTime;
    public float effectRadius;
    public float coolDown;
    public bool deleteAfterUse = false;

    [SerializeField] AudioManager audioManager;
    [SerializeField] VisualEffect vfx;

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
        audioManager.Play("Trigger");
        vfx.Play();
        isCoolingDown = true;

        PlayerAttributes.boostCooldown = coolDown;
        PlayerAttributes.boostSpeed += speedBoostAmount;
        float t = 0;

        while ( t < speedBoostTime) {
            t += Time.deltaTime;
            yield return null;
        }

        PlayerAttributes.boostSpeed -= speedBoostAmount;

        if (deleteAfterUse)
            Destroy(this.gameObject);
        yield return new WaitForSeconds(coolDown);
        isCoolingDown = false;

    }

}
