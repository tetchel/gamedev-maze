using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour {

    public Vector3 playerStartingPosition;
    public GameObject player;

    public GameObject newHighscoreDisplay;

    public InGameScore scoreDisplay;
    public Text yourScore;

    public Text highscoreNotify;
    public InputField nameInput;
    public Button enterHighscoreButton;
    public GameObject endOfGameButtons;

    public Pauser pauser;

    // Use this for initialization
    void Start () {
        newHighscoreDisplay.gameObject.SetActive(false);
	}
    
    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.name == Constants.PLAYER_NAME) {
            //Debug.Log("Good job");
            pauser.setGameOver(true);

            float score = scoreDisplay.getScore();
            int ranking = ScoreManager.instance().getRanking(score);
            yourScore.text = "You made it out in " + System.Math.Round(score, 2) + " seconds!";
            
            if(ranking != -1) {
                newHighscoreDisplay.gameObject.SetActive(true);
                highscoreNotify.text = "Your time is the " + ordinal(ranking + 1) + " best time!";

                // Hide the Play Again / Exit to Menu buttons to force them to enter a name
                endOfGameButtons.SetActive(false);
            }
        }
    }

    private string ordinal(int i) {
        // only handle 1-10
        if(i == 1) {
            return "1st";
        }
        else if(i == 2) {
            return "2nd";
        }
        else if(i == 3) {
            return "3rd";
        }
        else {
            return i + "th";
        }
    }

    public void onEnterHighscore() {
        string name = nameInput.text;
        if(string.IsNullOrEmpty(name)) {
            name = "NO1";
        }

        //Debug.Log("The name is " + name + " and the score is " + scoreDisplay.getScore());

        ScoreManager.instance().tryInsertHighscore(scoreDisplay.getScore(), name);

        enterHighscoreButton.interactable = false;

        endOfGameButtons.SetActive(true);
    }
}
