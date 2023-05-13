using UnityEngine;
using UnityEngine.VFX;

public class RecoveryZone : MonoBehaviour
{
    public float healSpeed = 25f;
    public float recoveryRadius = 20f;
    private SphereCollider col => GetComponent<SphereCollider>();
    private VisualEffect effect => GetComponent<VisualEffect>();

    private void OnValidate()
    {
        if (col != null)
            col.radius = recoveryRadius;

        if (effect != null)
            effect.SetFloat("Radius", recoveryRadius);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerAttributes.Recover();
    }



}
