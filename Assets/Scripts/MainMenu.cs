using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void playWithComputer()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    public void Quit()
    {
        Application.Quit();
    }

    
}
