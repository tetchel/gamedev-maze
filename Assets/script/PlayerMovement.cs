using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {

    private Animator animator;
    private Transform playerTransform;
    private CharacterController controller;

    public Text tutorialDisplay;

    public int forwardSpeed = 5;
    public int backwardSpeed = 3;
    public int horizontalSpeed = 2;
    public float sprintRatio = 1.15f;

    private const int TURN_SPEED = 4;

    private float gravity;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        StartCoroutine(showTutorial());
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

        Vector3 inputDir = Vector3.zero;
        float speed = 0;

        // forward 
        if (vert > 0.1) {
            inputDir = playerTransform.forward;

            if(sprint > 0.1) {
                speed = forwardSpeed * sprintRatio;
            }
            else {
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

    // Show instructions for a brief time
    IEnumerator showTutorial() {
        tutorialDisplay.text = "WASD and mouse to move.\nFire1 (shift) to sprint!";
        yield return new WaitForSeconds(6);
        tutorialDisplay.enabled = false;
    }

    // Apply gravity and move the player in the given direction at the given speed
    private void move(Vector3 direction, float speed) {
        gravity -= 9.81f * Time.deltaTime;
        direction.y += gravity;

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