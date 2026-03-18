using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string savePath;

    protected override void Awake()
    {
        base.Awake();

        savePath = Path.Combine(
            Application.persistentDataPath,
            "save.json"
        );

        Debug.Log("SavePath: " + savePath);
    }

    public void SaveGame()
    {
        SaveFileData file = new SaveFileData();

        // ===== 收集各系统数据 =====

        file.dungeon = DungeonManager.Instance.GetSaveData();

        string json = JsonUtility.ToJson(file, true);

        File.WriteAllText(savePath, json);

        Debug.Log("保存成功");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("没有存档");
            return;
        }

        string json = File.ReadAllText(savePath);

        SaveFileData file =
            JsonUtility.FromJson<SaveFileData>(json);

        // ===== 加载各系统 =====

        DungeonManager.Instance.LoadSaveData(
            file.dungeon
        );

        Debug.Log("读取成功");
    }
}