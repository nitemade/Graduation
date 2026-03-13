using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<GameManager>
{
    public GameObject mainMenu;
    public void StartGame()
    {
        GameManager.Instance.StartGame();
        mainMenu.SetActive(false);
    }
}
