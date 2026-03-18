using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    private int seed;
    public void StartGame()
    {
        Debug.Log("Game Start");
        seed = Random.Range(0, 1000000);
        

        DungeonManager.Instance.Generate(seed);
    }


    public void SaveGame()
    {
        SaveManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        SaveManager.Instance.LoadGame();
    }
}
