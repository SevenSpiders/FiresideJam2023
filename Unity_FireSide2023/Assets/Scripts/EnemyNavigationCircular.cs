using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class EnemyNavigationCircular : MonoBehaviour
{
    [Header("Navigation")]
    public float circleRadius = 5;
    [Range(.1f, 2f)]
    public float detail = 1;

    [Header("Navigation Agent")]
    public float radius = 1f;
    public float height = 1f;
    public float moveSpeed = 3f;
    public float turnSpeed = 120f;
    public float acceleration = 8f;
    public float stoppingDistance = 0f;
    public bool  autoBraking =true;


    private List<Vector3> navPositions = new();
    private NavMeshAgent agent;
    private int curWayPoint = 0;

    private void Awake() {


        navPositions = GetNavPositions();

        if (!(navPositions.Count > 0))
            return;

        agent = AddNavMeshAgent();
        agent.destination = navPositions[0];
    }

    private void Update()
    {
        if (!(navPositions.Count > 0))
            return;

        float dist = agent.remainingDistance;
        bool invalid = agent.pathStatus == NavMeshPathStatus.PathInvalid || dist == Mathf.Infinity;
        bool complete = agent.pathStatus == NavMeshPathStatus.PathComplete;


        if (!invalid && complete && dist <= stoppingDistance) {
            curWayPoint = (curWayPoint + 1) % navPositions.Count;
            agent.destination = navPositions[curWayPoint];
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

        // WayPath
        if (detail <= 0 || circleRadius <= 0)
            return;

        Gizmos.color = Color.yellow;

        if (!Application.isPlaying) {
            foreach (Vector3 p in GetNavPositions()) {
                Gizmos.DrawWireSphere(p, .1f);
            }
        }

        else {
            foreach (Vector3 p in navPositions) {
                Gizmos.DrawWireSphere(p, .1f);
            }
        }
    }
#endif


    private List<Vector3> GetNavPositions()
    {
        List<Vector3> allPositions = new();
        float curDetail = circleRadius * detail * 8;

        for (int i = 0; i < (int)curDetail; i++)
        {
            float x = Mathf.Cos(i / curDetail * (Mathf.PI * 2));
            float y = Mathf.Sin(i / curDetail * (Mathf.PI * 2));
            Vector3 curPos = new(transform.position.x + x * circleRadius, transform.position.y, transform.position.z + y * circleRadius);

            bool foundPos = NavMesh.SamplePosition(curPos, out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (foundPos) {
                Vector3 navPos = hit.position;
                allPositions.Add(navPos);
            }

        }

        return allPositions;

    }

    private NavMeshAgent AddNavMeshAgent()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>() == null ? this.AddComponent<NavMeshAgent>(): GetComponent<NavMeshAgent>();
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
