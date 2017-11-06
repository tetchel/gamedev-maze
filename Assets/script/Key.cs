using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour {

    private static List<string> gottenKeys = new List<String>();

    private Text notifyText;

    private Renderer keyRenderer;

    void Start() {
        keyRenderer = GetComponent<Renderer>();
        keyRenderer.material.color = Color.yellow;
        notifyText = GameObject.Find("notifyText").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider collid) {
        if (collid.gameObject.name == Constants.PLAYER_NAME) {
            string keyIndex = name.Substring("key".Length);
            gottenKeys.Add(keyIndex);

            StartCoroutine(showGotKey(keyIndex));
            // Hide the key
            keyRenderer.enabled = false;
            //Debug.Log("Keys " + string.Join(", ", gottenKeys.ToArray()));
        }
    }

    // Show a message for 1 second saying the key was obtained
    IEnumerator showGotKey(string keyIndex) {
        notifyText.text = "Got Key #" + keyIndex;
        yield return new WaitForSeconds(2);
        notifyText.text = "";
    }

    public static List<string> getGottenKeys() {
        return gottenKeys;
    }

    public static void restoreAllKeys() {
        foreach(Key key in FindObjectsOfType<Key>()) {
            key.gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
