using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshUpdater : MonoBehaviour
{
    private NavMeshSurface surf;
    public float tick = .1f;

    private void Awake() {
        surf = GetComponent<NavMeshSurface>() == null ? null : GetComponent<NavMeshSurface>();
        if (surf == null)
            return;
        StartCoroutine(UpdateNavMeshSurface(tick));

    }

    private IEnumerator UpdateNavMeshSurface(float tick = .1f)
    {
        yield return new WaitForSeconds(tick);
        surf.BuildNavMesh();
        StartCoroutine(UpdateNavMeshSurface(tick));
    }
}
