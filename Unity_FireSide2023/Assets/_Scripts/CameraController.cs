using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform player;
    [SerializeField] Transform playerObj;
    [SerializeField] Rigidbody rb;

    [SerializeField] float rotationSpeed;

    [SerializeField] Transform combatLookAt;

    [SerializeField] GameObject thirdPersonCam;
    [SerializeField] GameObject combatCam;
    [SerializeField] GameObject topDownCam;

    [SerializeField] CameraStyle currentStyle;

    public enum CameraStyle {
        Basic   = 0,
        Combat  = 1,
        Topdown = 2,
    }


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }





    void Update() {

        #if UNITY_EDITOR
            // switch styles
            if (Input.GetKeyDown("1")) SwitchCameraStyle(CameraStyle.Basic);
            if (Input.GetKeyDown("2")) SwitchCameraStyle(CameraStyle.Combat);
            if (Input.GetKeyDown("3")) SwitchCameraStyle(CameraStyle.Topdown);
        #endif


        Vector3 camPos = transform.position;

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(camPos.x, player.position.y, camPos.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown) {

            float verticalInput = PlayerController.movementInput.x;
            float horizontalInput = PlayerController.movementInput.y;
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // if (inputDir != Vector3.zero)
            //     playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if(currentStyle == CameraStyle.Combat) {

            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(camPos.x, combatLookAt.position.y, camPos.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }







    void SwitchCameraStyle(CameraStyle newStyle) {

        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }
}
