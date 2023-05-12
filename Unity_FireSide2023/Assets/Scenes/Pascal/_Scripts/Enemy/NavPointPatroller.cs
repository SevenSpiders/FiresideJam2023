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

            for (int i=0; i<numEnemies; i++) {
                EnemyMovementController enemy = Instantiate(enemyPrefabs[0], transform.position, transform.rotation, transform);
                enemy.movementSpeed = movementSpeed;
                enemy.movementType = movementType;
                enemy.a_TargetReached += HandleNavPointReached;
                enemies.Add(enemy);

                // set initial position
                float t = (float)i/ (float)numEnemies;
                int waypointIndex = Mathf.FloorToInt(t * (navPoints.Count - 1));

                // Calculate the interpolated position between two waypoints
                Vector3 startPos = navPoints[waypointIndex].position;
                Vector3 endPos = navPoints[waypointIndex + 1].position;
                // enemy.transform.position = Vector3.Lerp(startPos, endPos, t);
                PlaceEnemy(enemy, i);
                enemy.MoveToPoint(navPoints[waypointIndex + 1].position);
            }
        }

        void PlaceEnemy(EnemyMovementController enemy, int idx) {

            if (idx == 0) {
                enemy.transform.position = navPoints[0].position;
                return;
            }

            float distPlacement = TotalDistance()/ numEnemies * idx;

            float dist;
            int navIdx = 0;

            while (idx < navPoints.Count) {
                int idxNext = (navIdx + 1)% navPoints.Count;
                float pointDistance = Vector3.Distance(navPoints[idx].position, navPoints[idxNext].position);

                float distRest = distPlacement - pointDistance;

                if (distRest > 0 ) {
                    navIdx += 1;
                    distPlacement = distRest;
                } else {
                    float fraction = distRest/ pointDistance;
                    Vector3 dir = navPoints[idxNext].position - navPoints[idx].position;
                    Vector3 point = navPoints[idx].position + dir* fraction;
                    enemy.transform.position = point;
                    return;
                }
            }
        }


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

