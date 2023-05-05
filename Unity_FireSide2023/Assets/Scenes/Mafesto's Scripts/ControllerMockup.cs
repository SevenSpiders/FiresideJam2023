using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMockup : MonoBehaviour
{

    private float horizontalInput, verticalInput;

    [SerializeField] float moveSpeed = 5.0f;


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector2.right * horizontalInput * Time.deltaTime * moveSpeed);
        transform.Translate(Vector2.up * verticalInput * Time.deltaTime * moveSpeed);
    }
}
