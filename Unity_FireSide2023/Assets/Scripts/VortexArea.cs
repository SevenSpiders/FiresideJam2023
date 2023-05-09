using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexArea : MonoBehaviour
{
    public float pullForce = 7;
    public float radius = 20;
    private void OnTriggerStay(Collider other) {
        if (other == null)
            return;

        Vector3 dir = (transform.position - other.transform.position).normalized;
        float dist = Vector3.Distance(transform.position, other.transform.position);
        float mul = 1-Mathf.Abs(dist / radius);
        int inEye = dist < 1.5f ? 0 : 1;
        Vector3 finalForce = (dir * pullForce * mul * inEye) / Time.deltaTime;
        other.attachedRigidbody.AddForce(finalForce);
    }

    private void OnValidate() {
        transform.localScale = new(radius, radius, radius);
    }
}
