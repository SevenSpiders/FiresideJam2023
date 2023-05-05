using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMockup : MonoBehaviour
{

    private float horizontalInput, verticalInput;
    private Rigidbody2D playerRb;

    [SerializeField] float moveSpeed = 5.0f;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // playerRb.velocity = new Vector2(horizontalInput * moveSpeed * Time.deltaTime, verticalInput * moveSpeed * Time.deltaTime);

        playerRb.AddRelativeForce(Vector2.up * moveSpeed * verticalInput, ForceMode2D.Force);
        playerRb.AddRelativeForce(Vector2.right * moveSpeed * horizontalInput, ForceMode2D.Force);
    }
}
