using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Island");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
