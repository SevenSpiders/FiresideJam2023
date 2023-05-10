using UnityEngine;
using UnityEngine.VFX;

public class RecoveryZone : MonoBehaviour
{
    public float healSpeed = 25f;
    public float radius = 20f;
    private SphereCollider col => GetComponent<SphereCollider>();
    private VisualEffect effect => GetComponent<VisualEffect>();

    private void OnValidate()
    {
        col.radius = radius;
        if (effect != null)
            effect.SetFloat("Radius", radius);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerAttributes.Recover();
    }



}
