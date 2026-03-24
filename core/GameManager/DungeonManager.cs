using System;
using System.Collections;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>, ISaveable<DungeonSaveData>
{
    public int seed;
    protected override void Awake()
    {
        base.Awake();
    }

    public DungeonGenerator generator;


    public void Generate(int seed)
    {

        generator.ClearDungeon();

        this.seed = seed;

        generator.Generate(seed);
        RoomManager.Instance.ResetRooms();

    }

    public void Generate(DungeonSaveData data)
    {

        generator.ClearDungeon();

        this.seed = data.seed;

        generator.Generate(data);
        RoomManager.Instance.ResetRooms();

    }


    public DungeonSaveData GetSaveData()
    {
        DungeonSaveData data = new DungeonSaveData();

        data.seed = seed;

        data.currentRoomID = RoomManager.Instance.currentRoom.roomID;

        data.rooms = RoomManager.Instance.GetRoomSaveData();

        return data;
    }

    public void LoadSaveData(DungeonSaveData data)
    {
        seed = data.seed;

        Generate(data);

        StartCoroutine(LoadRoomState(data));

    }

    IEnumerator LoadRoomState(DungeonSaveData data)
    {
        // 等一帧，等房间生成完成
        yield return null;

        // 恢复房间状态
        RoomManager.Instance.ApplySaveData(data);

        // 恢复当前房间
        RoomManager.Instance.SetCurrentRoom(data.currentRoomID);
    }


}