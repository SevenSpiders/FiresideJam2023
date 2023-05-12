using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal
{

    public class NavPointPatroller : MonoBehaviour
    {


        [Header("Enemies")]
        [SerializeField] int numEnemies;
        [SerializeField] float movementSpeed = 10f;
        [SerializeField] EnemyMovementController.MovementType movementType;
        [SerializeField] List<EnemyMovementController> enemyPrefabs;

        [Header("Navigation")]
        [SerializeField] protected List<Transform> navPoints;
        [SerializeField] bool clockWise;
        [SerializeField] bool loop;


        protected List<EnemyMovementController> enemies;




        protected virtual void Start() {
            enemies = new();
            UpdatePoints();

            float spacing = TotalDistance() / numEnemies;

            for (int i=0; i<numEnemies; i++) {
                EnemyMovementController enemy = Instantiate(enemyPrefabs[0], transform.position, transform.rotation, transform);
                enemy.movementSpeed = movementSpeed;
                enemy.movementType = movementType;
                enemy.a_TargetReached += HandleNavPointReached;
                enemies.Add(enemy);

                float distanceTravelled = i * spacing;
                int idx;
                Vector3 position = GetPositionOnPath(distanceTravelled, clockWise, out idx);

                enemy.transform.position = position;
                enemy.MoveToPoint(navPoints[idx].position);
            }
        }


        Vector3 GetPositionOnPath(float distanceTravelled, bool clockWise, out int idx) {
            for (int i = 0; i+1 < navPoints.Count; i++) {
                // Calculate the distance between two path points
                float distance = Vector3.Distance(navPoints[i].position, navPoints[i+1].position);

                // If the current distance is greater than the distance travelled,
                // interpolate the position between the two path points
                if (distanceTravelled < distance) {
                    float t = distanceTravelled / distance;
                    idx = i+1;
                    return Vector3.Lerp(navPoints[i].position, navPoints[i+1].position, t);
                }

                distanceTravelled -= distance;
            }

            // If the distance travelled is greater than the total path length,
            // return the last path point position
            idx = navPoints.Count -1;
            return navPoints[idx].position;
        }




        // void PlaceEnemy(EnemyMovementController enemy, int idx) {

        //     if (idx == 0) {
        //         enemy.transform.position = navPoints[0].position;
        //         return;
        //     }

        //     float distPlacement = TotalDistance()/ numEnemies * idx;

        //     float dist;
        //     int navIdx = 0;

        //     while (idx < navPoints.Count) {
        //         int idxNext = (navIdx + 1)% navPoints.Count;
        //         float pointDistance = Vector3.Distance(navPoints[idx].position, navPoints[idxNext].position);

        //         float distRest = distPlacement - pointDistance;

        //         if (distRest > 0 ) {
        //             navIdx += 1;
        //             distPlacement = distRest;
        //         } else {
        //             float fraction = distRest/ pointDistance;
        //             Vector3 dir = navPoints[idxNext].position - navPoints[idx].position;
        //             Vector3 point = navPoints[idx].position + dir* fraction;
        //             enemy.transform.position = point;
        //             return;
        //         }
        //     }
        // }


        float TotalDistance() {
            float dist =0;
            for (int i =0; i< navPoints.Count; i++) {
                if (loop && i == navPoints.Count) return dist;
                int idx = (i+1)%navPoints.Count;
                dist += Vector3.Distance(navPoints[i].position, navPoints[idx].position);
            }
            return dist;
        }




        [ContextMenu("Update")]
        void UpdatePoints() {
            navPoints = new();
            foreach (Transform child in transform) {
                navPoints.Add(child);
            }

        }

        

        


        protected EnemyMovementController GetFish(int fishID) {
            for (int i=0; i< enemies.Count; i++) {
                if (enemies[i].id == fishID) return enemies[i];
            }
            return null;
        }


        protected int GetNavIdx(Vector3 pos) {
            for (int i=0; i< navPoints.Count; i++) {
                if (navPoints[i].position == pos) return i;
            }
            return -1;
        }

        protected virtual void HandleNavPointReached(int id, Vector3 pos) {
            EnemyMovementController fish = GetFish(id);
            int idx = GetNavIdx(pos);

            if (idx <0 || fish == null) return;

            Vector3 nextPoint = navPoints[(idx +1)%navPoints.Count].position;
            fish.MoveToPoint(nextPoint);
        }



        //--------------------------------------------------------------------

        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 2f);

            Gizmos.color = Color.yellow;

            foreach (var point in navPoints) {
                Gizmos.DrawWireSphere(point.position, 2f);
            }
        }

    }// class
}


/*

using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<Transform> navPoints;  // List of path points
    [SerializeField] private GameObject objectPrefab;    // Object prefab to place on the path
    [SerializeField] private float distanceBetweenObjects = 1f;   // Distance between objects on the path

    private void Start()
    {
        // Place objects on the path
        PlaceObjectsOnPath();
    }

    private void PlaceObjectsOnPath()
    {
        float totalPathLength = GetTotalPathLength();

        // Calculate the number of objects to be placed
        int numObjects = Mathf.FloorToInt(totalPathLength / distanceBetweenObjects);

        // Calculate the distance between each object
        float spacing = totalPathLength / numObjects;

        // Place objects on the path with equal distance between them
        for (int i = 0; i < numObjects; i++)
        {
            // Calculate the distance travelled along the path
            float distanceTravelled = i * spacing;

            // Get the position and rotation of the object
            Vector3 position = GetPositionOnPath(distanceTravelled);
            Quaternion rotation = GetRotationOnPath(distanceTravelled);

            // Instantiate the object
            Instantiate(objectPrefab, position, rotation, transform);
        }
    }

    private float GetTotalPathLength()
    {
        float totalLength = 0f;

        for (int i = 0; i < navPoints.Count - 1; i++)
        {
            // Calculate the distance between two path points
            float distance = Vector3.Distance(navPoints[i].position, navPoints[i + 1].position);

            totalLength += distance;
        }

        return totalLength;
    }

    private Vector3 GetPositionOnPath(float distanceTravelled)
    {
        for (int i = 0; i < navPoints.Count - 1; i++)
        {
            // Calculate the distance between two path points
            float distance = Vector3.Distance(navPoints[i].position, navPoints[i + 1].position);

            // If the current distance is greater than the distance travelled,
            // interpolate the position between the two path points
            if (distanceTravelled < distance)
            {
                float t = distanceTravelled / distance;
                return Vector3.Lerp(navPoints[i].position, navPoints[i + 1].position, t);
            }

            distanceTravelled -= distance;
        }

        // If the distance travelled is greater than the total path length,
        // return the last path point position
        return navPoints[navPoints.Count - 1].position;
    }

    private Quaternion GetRotationOnPath(float distanceTravelled)
    {
        for (int i = 0; i < navPoints.Count - 1; i++)
        {
            // Calculate the distance between two path points
            float distance = Vector3.Distance(navPoints[i].position, navPoints[i + 1].position);

            // If the current distance is greater than the distance travelled,
            // interpolate the rotation between the two path points
            if (distanceTravelled < distance)
            {
                float t = distanceTravelled / distance;
                Quaternion rotation = Quaternion.Lerp(navPoints[i].rotation, navPoints[i + 1].rotation, t);

                // Adjust the rotation based on the path direction
                Vector3 direction = navPoints[i + 1].position - navPoints[i].position;
                rotation *= Quaternion.FromToRotation(Vector3.right, direction);

                return rotation;
            }

            distanceTravelled -= distance;
*/