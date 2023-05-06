using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MA_SellZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MA_PlayerAttributes>() == null)
            return;

        other.GetComponent<MA_PlayerAttributes>().EnableSellButton(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MA_PlayerAttributes>() == null)
            return;

        other.GetComponent<MA_PlayerAttributes>().EnableSellButton(false);
    }

}
