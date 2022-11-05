using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCard : MonoBehaviour
{
    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
