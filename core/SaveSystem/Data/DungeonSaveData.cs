using System;
using System.Collections.Generic;

[System.Serializable]
public class DungeonSaveData
{
    public int seed;
    public int currentRoomID;

    public List<RoomSaveData> rooms;
}