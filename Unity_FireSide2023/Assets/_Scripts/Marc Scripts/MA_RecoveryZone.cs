using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MA_RecoveryZone : MonoBehaviour
{
    public float healSpeed = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MA_PlayerAttributes>() == null)
            return;

        MA_PlayerAttributes.isSafe = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MA_PlayerAttributes>() == null)
            return;

        MA_PlayerAttributes.isSafe = false;
    }


}
