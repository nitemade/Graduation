using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject statPanel;
    public GameObject mainMenu;
    public GameObject MinimapPanel;
    private bool isPaused = true;
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        HideMenu();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                
                HideMenu();
            }
            else
            {
                
                ShowMenu();
            }
        }
    }

    public void ShowMenu()
    {
        Time.timeScale = 0;
        isPaused = true;
        mainMenu.SetActive(true);
        statPanel.SetActive(false);
        MinimapPanel.SetActive(false);
    }

    public void HideMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        mainMenu.SetActive(false);
        statPanel.SetActive(true);
        MinimapPanel.SetActive(true);
    }

    public bool IsPaused => isPaused;
}
