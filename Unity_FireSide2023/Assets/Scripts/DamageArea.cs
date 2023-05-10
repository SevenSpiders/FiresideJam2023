#if UNITY_EDITOR
using Cinemachine.Utility;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.UIElements;

public class DamageArea : MonoBehaviour
{
    public enum Shape { Sphere,Cube}

    [Header("Define Area")]
    public Shape areaShape = Shape.Cube;
    public float size = 2;
    [Header("Instant Damage With Countdown Or Damage Per Second")]
    public float countdown = 5;
    public bool useCountdown = false;
    public float damage = 20;



    private void Awake()
    {
        if (areaShape == Shape.Cube) {
            BoxCollider col = this.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = Vector3.one * size;
        }

        if (areaShape == Shape.Sphere) {
            SphereCollider col = this.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = size;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || !useCountdown)
            return;

        StartCoroutine(DamageWithCountdown());
    }


    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player") || !useCountdown)
            return;

        StopAllCoroutines();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || useCountdown)
            return;

        PlayerAttributes.DamageOverTime(damage);
    }

    private IEnumerator DamageWithCountdown() {

        PlayerAttributes.Damage(damage);
        float t = 0;
        
        while(t < countdown) {
            t += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(DamageWithCountdown());
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (SceneView.lastActiveSceneView.camera == null)
            return;

        Camera sceneCam = SceneView.lastActiveSceneView.camera;
        float dist = Vector3.Distance(sceneCam.transform.position, transform.position);

        if (dist > 50)
            return;

        if( areaShape == Shape.Cube) {
            Handles.DrawWireCube(transform.position, Vector3.one * size);
        }

        if (areaShape == Shape.Sphere) {
            Handles.DrawWireDisc(transform.position,Vector3.up,size);
            Handles.DrawWireDisc(transform.position, Vector3.forward, size);
            Handles.DrawWireDisc(transform.position, Vector3.right, size);
        }
    }
#endif


}
