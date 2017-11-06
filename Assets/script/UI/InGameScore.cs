using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InGameScore : MonoBehaviour {

    public Text scoreDisplay;

    private float score = 0;

    // Use this for initialization
    void Start() {
        scoreDisplay = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        score += Time.deltaTime;
        scoreDisplay.text = "" + System.Math.Round(score);
    }

    public float getScore() {
        return score;
    }
}
