using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private MusicManager musicManager;

    public static bool isPaused = false;
    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private FMODUnity.StudioEventEmitter pauseSoundEmitter;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
        
    }

    void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        musicManager.SwitchToLevelMusic();
        pauseSoundEmitter.Play();
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        musicManager.SwitchToPauseMusic();
        pauseSoundEmitter.Play();
    }

    public void OnDisable() {
        Time.timeScale = 1f;
    }
}
