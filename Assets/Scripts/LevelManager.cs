using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void EndCard() {
        SceneManager.LoadScene("End Card");
    }

    public void Level1() {
        SceneManager.LoadScene("Level 1");
    }

    public void Level2() {
        SceneManager.LoadScene("Level 2");
    }

    public void Level3() {
        SceneManager.LoadScene("Level 3");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ExecuteCommand(string command) {
        switch(command) {
            case "_Main Menu":
                MainMenu();
                break;
            case "_End Card":
                EndCard();
                break;
            case "_Level 1":
                Level1();
                break;
            case "_Level 2":
                Level2();
                break;
            case "_Level 3":
                Level3();
                break;
            case "_Exit Game":
                ExitGame();
                break;
            default:
                throw new ArgumentException("Invalid command string given.");
        }
    }
}
