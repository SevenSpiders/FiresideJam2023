using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int value = 1;
    public float collectRadius = 1;
    private SphereCollider col;
    private void Awake() {
        col = AddSphereCollider();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }

    private SphereCollider AddSphereCollider() {
        // Get existing collider or create one and apply values
        SphereCollider col = GetComponent<SphereCollider>() != null ? GetComponent<SphereCollider>() : this.gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = collectRadius;

        return col;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        collectEvent();
    }

    private void collectEvent()
    {
        // What Should it Do??
        Destroy(this.gameObject);
    }
}
