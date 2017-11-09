using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {

    private Animator animator;
    private Transform playerTransform;
    private CharacterController controller;

    public int forwardSpeed = 5;
    public int backwardSpeed = 3;
    public int horizontalSpeed = 2;

    private const int TURN_SPEED = 4;

    private const int GRAVITY = 30;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    void LateUpdate() {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        
        // Rotate the player based on mouse movement
        playerTransform.Rotate(0, Input.GetAxis("Mouse X") * Constants.TURN_SPEED * Time.deltaTime, 0);

        animate(horiz, vert);
    }
     
    private void animate(float horiz, float vert) {
        // Animator uses the exact same logic as below to pick animation direction

        animator.SetFloat("vert", vert);
        animator.SetFloat("horiz", horiz);
        float sprint = isSprinting();
        animator.SetFloat("sprint", sprint);
        Debug.Log("sprint " + sprint);

        Vector3 inputDir = Vector3.zero;
        float speed = 0;

        // forward 
        if (vert > 0.1) {
            inputDir = playerTransform.forward;

            if(sprint > 0.1) {
                Debug.Log("doing a sprint speed");
                speed = forwardSpeed * 1.25f;
            }
            else {
                Debug.Log("not doing a sprint speed");
                speed = forwardSpeed;
            }
        }
        // backward
        else if (vert < -0.1) {
            inputDir = -playerTransform.forward;
            speed = backwardSpeed;
        }
        // right
        else if (horiz > 0.1) {
            inputDir = playerTransform.right;
            speed = horizontalSpeed;
        }
        // left
        else if (horiz < -0.1) {
            inputDir = -playerTransform.right;
            speed = horizontalSpeed;
        }

        // Always move - This will apply gravity even if there is no input.
        move(inputDir, speed);
    }

    // Apply gravity and move the player in the given direction at the given speed
    private void move(Vector3 direction, float speed) {
        // "gravity" - looks really bad - should accelerate instead
        direction.y -= GRAVITY * Time.deltaTime;

        direction.x *= speed;
        direction.z *= speed;
        controller.Move(direction * Time.deltaTime);
    }

    private float isSprinting() {
        if (Input.GetButton("Fire1")) {
            return 0.2f;
        }
        else {

            return 0.0f;
        }
    }
}