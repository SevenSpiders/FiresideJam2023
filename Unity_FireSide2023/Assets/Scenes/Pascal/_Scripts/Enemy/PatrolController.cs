using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pascal
{

    public class PatrolController : MonoBehaviour
    {
        [SerializeField] int numEnemies;
        [SerializeField] float radius;
        [SerializeField] List<EnemyMovementController> enemyPrefabs;
        [SerializeField] float rotationRate = 0.1f;


        int numNavPoints =10;
        public List<Vector3> navPoints;

        List<EnemyMovementController> enemies;
        float deg;




        void Start() {
            enemies = new();
            navPoints = new();
            // numNavPoints = numEnemie;

            SetupNavPoints();

            for (int i=0; i<numEnemies; i++) {
                EnemyMovementController _e = Instantiate(enemyPrefabs[0], transform.position, transform.rotation, transform);
                enemies.Add(_e);
                _e.a_TargetReached += HandleNavPointReached;
                _e.MoveToPoint(navPoints[i]);
                Debug.Log(_e);
            }
            // UpdatePositions();
        }


        void SetupNavPoints() {
            for (int i = 0; i < numNavPoints; i++) {

                float angle = i * (360f / numNavPoints);
                float radian = angle * Mathf.Deg2Rad;
                float x = Mathf.Sin(radian) * radius;
                float z = Mathf.Cos(radian) * radius;

                navPoints.Add(transform.position + new Vector3(x, 0, z));
            }
        }



        void UpdatePositions() {
            for (int i = 0; i < numEnemies; i++) {
                
                deg += Time.deltaTime*rotationRate;

                EnemyMovementController enemy = enemies[i];
                float angle = i * (360f / numEnemies)+deg;
                float radian = angle * Mathf.Deg2Rad;
                float x = Mathf.Sin(radian) * radius;
                float z = Mathf.Cos(radian) * radius;

                enemy.MoveToPoint(transform.position + new Vector3(x, 0, z));
            }
        }

        

        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);

            Gizmos.color = Color.yellow;

            foreach (var point in navPoints) {
                Gizmos.DrawWireSphere(point, 2f);
            }
        }


        EnemyMovementController GetFish(int fishID) {
            for (int i=0; i< enemies.Count; i++) {
                if (enemies[i].id == fishID) return enemies[i];
            }
            return null;
        }


        int GetNavIdx(Vector3 pos) {
            for (int i=0; i< navPoints.Count; i++) {
                if (navPoints[i] == pos) return i;
            }
            return -1;
        }

        void HandleNavPointReached(int id, Vector3 pos) {
            EnemyMovementController fish = GetFish(id);
            int idx = GetNavIdx(pos);

            if (idx <0 || fish == null) return;

            Vector3 nextPoint = navPoints[(idx +1)%numNavPoints];
            fish.MoveToPoint(nextPoint);
        }

    }
}

