using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<GameManager>
{
    public GameObject statPanel;
    public GameObject mainMenu;
    private bool isPaused = true;
    public void StartGame()
    {
        GameManager.Instance.StartGame();
        isPaused = false;
        mainMenu.SetActive(false);
        statPanel.SetActive(true);
    }

    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        isPaused = false;
        mainMenu.SetActive(false);
        statPanel.SetActive(true);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                mainMenu.SetActive(false);
                statPanel.SetActive(true);
                isPaused = false;
            }
            else
            {
                mainMenu.SetActive(true);
                statPanel.SetActive(false);
                isPaused = true;
            }
        }
    }

    public void ShowMenu()
    {
        mainMenu.SetActive(true);
        statPanel.SetActive(false);
    }
}
