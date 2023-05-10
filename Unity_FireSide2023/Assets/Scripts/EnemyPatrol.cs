using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float patrolSpeed;
    private int currentPoint = 0;
    [SerializeField] Vector3[] patrolPoints;
    public float patrolPointRadius = 1f;
    private bool isFollowingPlayer;
    private GameObject playerTarget;



    private void Update() {

        if (!isFollowingPlayer)
            Patrol();
        else
            FollowPlayer();    
    }

    private void Patrol()
    {
        foreach( Vector3 p in patrolPoints) {
            while(Vector3.Distance(transform.position,transform.position + p) > patrolPointRadius) {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + p, patrolSpeed * Time.deltaTime);
                transform.LookAt(transform.position + p);
            }
        }

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
            Gizmos.DrawWireSphere(transform.position + p, patrolPointRadius);
    }
}

