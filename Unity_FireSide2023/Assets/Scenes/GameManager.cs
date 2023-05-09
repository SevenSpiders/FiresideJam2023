using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject playerLosePanel;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (MA_PlayerAttributes.health == 0)
        {
            MA_BoatController.movementEnabled = false;
            playerLosePanel.SetActive(true);
        }
    }
}
