using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float patrolSpeed;
    private int currentPoint = 0;
    [SerializeField] Transform[] patrolPoints;

    private bool isFollowingPlayer;

    private GameObject playerTarget;

    private void OnValidate()
    {
    }

    private void Update()
    {
        if (!isFollowingPlayer)
        {
            Patrol();
        }
        else
        {
            if (playerTarget != null)
                FollowPlayer();
        }
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, patrolSpeed * Time.deltaTime);

        // Check if the enemy has reached the current patrol point
        if (transform.position == patrolPoints[currentPoint].position)
        {
            // Move to the next patrol point
            currentPoint++;

            // Reset the index to loop back to the first patrol point
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, patrolSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BoatController>())
        {
            isFollowingPlayer = true;
            playerTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BoatController>())
        {
            isFollowingPlayer = false;
        }
    }
}

