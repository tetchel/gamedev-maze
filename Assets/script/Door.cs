using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    private GameObject player;

    private Text notifier;

    private const float ACTIVATION_RADIUS = 1f;
    private string doorIndex;

    private Vector3 closedPos;
    private Vector3 openPos;

    private bool open = false;
    private bool opening = false;
    private bool closing = false;

    private bool displayingMissingKeyMsg = false;

    public float speed = 2.5f; 

	// Use this for initialization
	void Start () {
        player = GameObject.Find("player");
        notifier = GameObject.Find("notifyText").GetComponent<Text>();

        closedPos = transform.position;
        openPos = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y, 
            transform.position.z);

        string possibleDoorIndex = name.Substring("door".Length);
        int doorIndexInt;
        if(!int.TryParse(possibleDoorIndex, out doorIndexInt)) {
            Debug.LogError("Door script attached to something not named door? " + name);
        }
        else {
            doorIndex = possibleDoorIndex;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(opening) {
            openCloseDoor(true);
            return;
        }

        if(closing) {
            openCloseDoor(false);
            return;
        }

        if (open) {
            return;
        }

        // If player is nearby
		if((transform.position - player.transform.position).magnitude <=
            (ACTIVATION_RADIUS + transform.position.y - player.transform.position.y)) {

            if (Key.getGottenKeys().Contains(doorIndex)) {
                opening = true;
                Key.getGottenKeys().Remove(doorIndex);
                //Debug.Log("closing a door");
            }
            else {
                //Debug.Log("no key for " + name);

                notifier.enabled = true;
                notifier.text = "Missing key #" + doorIndex;
                displayingMissingKeyMsg = true;

                //foreach(string s in Key.getGottenKeys()) {
                //    Debug.Log("key " + s);
                //}
            }
        }
        else if(displayingMissingKeyMsg) {
            notifier.enabled = false;
            displayingMissingKeyMsg = false;
        }
	}

    // Move the door closer to being open or closed. 
    // pass true to make the door more open each frame, false to make it more closed.
    void openCloseDoor(bool isOpening) {
        Vector3 destinationPos = isOpening ? openPos : closedPos;
        // If the door has not yet reached its resting position under the maze
        if(!Mathf.Approximately(
            (float) System.Math.Round(transform.position.y, 2), 
            (float) System.Math.Round(destinationPos.y, 2))) {

            // Retract the door
            transform.position = Vector3.MoveTowards(transform.position, destinationPos, speed * Time.deltaTime);
            //Debug.Log("Door retracting from " + transform.position.y + " to " + destinationPos);
        }
        else if (isOpening) {
            //Debug.Log("Done opening");
            // Finished opening
            opening = false;
            open = true;
        }
        else {
            //Debug.Log("Done closing");
            // finished closing
            closing = false;
            open = false;
        }
    }

    public void startCloseDoor() {
        closing = true;
    }

    public static void closeAllDoors() {
        foreach (Door door in FindObjectsOfType<Door>()) {
            door.startCloseDoor();
        }
    }
}
