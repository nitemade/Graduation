using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public PlayerSaveData player;

    public List<EnhancementSaveData> enhancements;

    public DungeonSaveData dungeon;

    public RoomSaveData room;
}