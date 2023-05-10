using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>() == null ? null : GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
            return;

        HitEvent(other.gameObject);
    }

    public void ShootProjectile(Vector3 worldStartPos, Vector3 worldEndPos, float speed = .4f, float timeOffset = .1f, float aimTime = 3)
    {
        if (rb == null)
            return;

        StartCoroutine(EShootProjectile(worldStartPos, worldEndPos, speed, timeOffset, aimTime));
    }

    private IEnumerator EShootProjectile(Vector3 worldStartPos,Vector3 worldEndPos, float speed, float timeOffset, float aimTime)
    {
        transform.position = worldStartPos;
        Rigidbody curRig = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(timeOffset);
        float t = 0;
        Vector3 aimDir = (worldEndPos - transform.position).normalized;

        while (t < aimTime) {
            t += Time.deltaTime;
            curRig.AddForce(speed * aimDir,ForceMode.Impulse);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void HitEvent(GameObject hitObject) {
        Destroy(hitObject.transform.parent.gameObject);
    }
}
