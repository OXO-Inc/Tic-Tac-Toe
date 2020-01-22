using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panel;
    public Button settings;
    public Button withComputer;
    public Button withHuman;
    public Button quit;
    public Dropdown dropdown;

    public void Start()
    {
        if (PlayerPrefs.HasKey("option") == false)
            PlayerPrefs.SetInt("option", 0);

        dropdown.value = PlayerPrefs.GetInt("option");

    }

    public void optionSelected(int index)
    {
        PlayerPrefs.SetInt("option", index);
    }

    public void playWithComputer()
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetInt("isAI", 1);
    }

    public void playWithHuman()
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetInt("isAI", 0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void openPanel()
    {
        panel.SetActive(true);
        settings.interactable = false;
        withComputer.interactable = false;
        withHuman.interactable = false;
        quit.interactable = false;
    }

    public void closePanel()
    {
        panel.SetActive(false);
        settings.interactable = true;
        withComputer.interactable = true;
        withHuman.interactable = true;
        quit.interactable = true;
    }

}
