using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyNavigationPath : MonoBehaviour
{
    [Header("Navigation")]
    public List<Vector3> navPositions;

    [Header("Navigation Agent")]
    public float radius = 1f;
    public float height = 1f;
    public float moveSpeed = 3f;
    public float turnSpeed = 120f;
    public float acceleration = 8f;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;

    private NavMeshAgent agent;
    private int curWayPoint = 0;
    private Vector3 startPos;

    private void Awake()
    {
        agent = AddNavMeshAgent();
        navPositions = GetNavPositions();

        if (navPositions.Count == 0)
            return;

        agent.destination = startPos + navPositions[0];
    }

    private void Update()
    {
        if (navPositions.Count == 0)
            return;

        float dist = agent.remainingDistance;
        bool invalid = agent.pathStatus == NavMeshPathStatus.PathInvalid || dist == Mathf.Infinity;
        bool complete = agent.pathStatus == NavMeshPathStatus.PathComplete;


        if (!invalid && complete && dist <= stoppingDistance)
        {
            curWayPoint = (curWayPoint + 1) % navPositions.Count;
            agent.destination = startPos + navPositions[curWayPoint];
        }

    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Agent
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, radius);
        Handles.DrawWireDisc(transform.position + Vector3.up * height, Vector3.up, radius);
        Handles.DrawLine(transform.position + Vector3.right * radius, transform.position + Vector3.right * radius + Vector3.up * height);
        Handles.DrawLine(transform.position + -Vector3.right * radius, transform.position + -Vector3.right * radius + Vector3.up * height);
        Handles.DrawLine(transform.position + Vector3.forward * radius, transform.position + Vector3.forward * radius + Vector3.up * height);
        Handles.DrawLine(transform.position + -Vector3.forward * radius, transform.position + -Vector3.forward * radius + Vector3.up * height);


        // WayPoints
        Gizmos.color = Color.yellow;

        if (Application.isPlaying)
            foreach (Vector3 p in navPositions)
            {
                Gizmos.DrawWireSphere(transform.position + p, .1f);
            }
        else
            foreach (Vector3 p in GetNavPositions())
            {
                Gizmos.DrawWireSphere(p, .1f);
            }
    }
#endif


    private List<Vector3> GetNavPositions()
    {
        List<Vector3> allPositions = new();

        foreach (Vector3 p in navPositions)
        {
            bool foundPos = NavMesh.SamplePosition(transform.TransformPoint(p), out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (foundPos)
            {
                Vector3 navPos = hit.position;
                allPositions.Add(navPos);
            }
        }
        return allPositions;
    }

    private NavMeshAgent AddNavMeshAgent()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>() == null ? this.AddComponent<NavMeshAgent>() : GetComponent<NavMeshAgent>();
        agent.baseOffset = 0f;
        agent.radius = radius;
        agent.height = height;
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = autoBraking;

        return agent;
    }
}
