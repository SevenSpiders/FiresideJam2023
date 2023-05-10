using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float patrolSpeed;
    private int currentPoint = 0;
    [SerializeField] Vector3[] patrolPoints;
    public float patrolPointRadius = 1f;
    private bool isFollowingPlayer;
    private GameObject playerTarget;
    private Vector3 startPos;
    private int currentPatrolPointIndex = 0; 


    private void Awake() {
        startPos = transform.position;
    }

    private void Update() {

        if (!isFollowingPlayer)
            Patrol();
        else
            FollowPlayer();    
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) 
            return;
        

        Vector3 targetPosition = startPos + patrolPoints[currentPatrolPointIndex];

        if (Vector3.Distance(transform.position, targetPosition) > patrolPointRadius) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);
            transform.LookAt(targetPosition);
        }
        else
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        
    }



    private void FollowPlayer() {
        if (playerTarget == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, patrolSpeed * Time.deltaTime);
        transform.LookAt(playerTarget.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            isFollowingPlayer = true;
            playerTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isFollowingPlayer = false;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Vector3 p in patrolPoints)
            Gizmos.DrawWireSphere(startPos + p, patrolPointRadius);
    }
}

