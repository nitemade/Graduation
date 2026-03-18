using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<GameManager>
{
    public GameObject mainMenu;
    private bool isPaused = true;
    public void StartGame()
    {
        GameManager.Instance.StartGame();
        isPaused = false;
        mainMenu.SetActive(false);
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
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                mainMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                mainMenu.SetActive(true);
                isPaused = true;
            }
        }
    }

    public void ShowMenu()
    {
        mainMenu.SetActive(true);
    }
}
