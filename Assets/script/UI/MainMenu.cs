using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Pauser pauser;

    public void onPlay() {
        SceneManager.LoadScene("scene");
        pauser.unPause();
    }

    public void onQuit() {
        Application.Quit();
    }
}
