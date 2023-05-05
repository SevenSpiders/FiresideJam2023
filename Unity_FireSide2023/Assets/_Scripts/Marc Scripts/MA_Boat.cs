using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MA_Boat : MonoBehaviour
{
    public List<Vector3> buoyancyPoints;
    public float waterLevel = 0;
    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>() != null ? GetComponent<Rigidbody>() : null;
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        foreach (Vector3 p in buoyancyPoints)
        {
            Vector3 worldPosition = transform.position + p;

            if (p.y < waterLevel)
            {
                float distance = Mathf.Abs(p.y - waterLevel);

            }
        }


         void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Vector3 p in buoyancyPoints) {
                Gizmos.DrawWireSphere(transform.position + p, .1f);
            }
        }
    }
}

