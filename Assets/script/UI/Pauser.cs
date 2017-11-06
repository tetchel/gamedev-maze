using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pauser : MonoBehaviour {

    public GameObject pauseMenu;

    private bool gameOver = false;

    private float escCooldownCounter = 0;
    private const float ESC_COOLDOWN = 40;

    // Use this for initialization
    void Start() {

        pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI() {
        Event e = Event.current;
        escCooldownCounter++;

        if(e.keyCode == KeyCode.Escape && escCooldownCounter >= ESC_COOLDOWN) {
            escCooldownCounter = 0;
            if(Time.timeScale == 0 && !gameOver) {
                unPause();
            }
            else {
                pause();
            }
        }
    }

    public void pause() {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenu.gameObject.SetActive(true);
    }

    public void unPause() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.gameObject.SetActive(false);
    }

    public void onPlayAgain() {
        gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        unPause();
    }

    public void onMainMenu() {
        gameOver = false;
        SceneManager.LoadScene("menu");
    }

    public void setGameOver(bool pGameOver) {
        gameOver = pGameOver;
        if(gameOver) {
            pause();
        }
        else {
            unPause();
        }
    }
}
