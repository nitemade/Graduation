using System;
using System.Collections.Generic;

[Serializable]
public class RoomSaveData
{
    public int currentRoomIndex;

    public List<int> clearedRooms = new();
}