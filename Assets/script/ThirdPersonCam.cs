using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour {
    
    public Transform player;

    // Should be removed once can rotate in Y
    public float aimHeight = 2f;

    // Difference between player location and camera location
    public Vector3 camToPlayerOffset;

    // Use this for initialization
    void Start () {
        // Since we're only using the mouse to rotate, we don't want a cursor, 
        // and we want to confine the cursor to the window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate () {
        cameraLookAtPlayer();
    }

    // Move the camera based on mouse movement, then look at the player
    void cameraLookAtPlayer() {
        // The mouseX scalar must (?) match the player's turn speed 
        camToPlayerOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Constants.TURN_SPEED * Time.deltaTime, 
            Vector3.up) * camToPlayerOffset;
        //camToPlayerOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * Constants.TURN_SPEED * Time.deltaTime, 
        //  Vector3.right) * camToPlayerOffset;

        transform.position = player.transform.position + camToPlayerOffset;

        Vector3 cameraLook = player.transform.position;
        cameraLook.y += aimHeight;
        transform.LookAt(cameraLook);

        // make sure transform is updated before this line
        adjustForCameraCollision(cameraLook);
    }

    void adjustForCameraCollision(Vector3 cameraLook) {
        // Cast ray from person back to the camera, testing if it hits anything before it hits the camera
        Vector3 lookToCamera = transform.position - cameraLook;

        RaycastHit hit;
        if (Physics.Raycast(cameraLook, lookToCamera, out hit, maxDistance: lookToCamera.magnitude)) {
            // move the camera to the point of the hit so that it is not obstructed anymore
            transform.position = hit.point;
        }
    }
}

