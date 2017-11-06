using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void onPlay() {
        SceneManager.LoadScene("scene");
    }

    public void onQuit() {
        Application.Quit();
    }
}
