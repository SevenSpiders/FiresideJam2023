using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CurrentsForceBoost : MonoBehaviour
{
    public float forceAmount;
    public Vector3 size;
    private BoxCollider col;

    private void Awake() {
        col = AddBoxCollider();
    }

    private void OnDrawGizmos() {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }

    private BoxCollider AddBoxCollider() {
        // Get existing collider or create one and apply values
        BoxCollider col = GetComponent<BoxCollider>() != null ? GetComponent<BoxCollider>() : this.gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = size;

        return col;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            PlayerAttributes.curCurrent = this.gameObject;
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") && PlayerAttributes.curCurrent == this.gameObject)
            other.attachedRigidbody.AddForce((transform.forward * forceAmount) * (1 / Time.deltaTime));
    }

}
