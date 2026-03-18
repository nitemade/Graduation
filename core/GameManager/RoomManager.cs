using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomManager : Singleton<RoomManager>
{
    public Room currentRoom;
    List<Room> rooms = new List<Room>();

    private Dictionary<Vector2, Door> doorPositionMap = new Dictionary<Vector2, Door>();

    protected override void Awake()
    {
        base.Awake();

        doorPositionMap.Clear();
        currentRoom = null;
    }

    public Room CreateRoom(GameObject obj, RoomType type)
    {
        Room old = obj.GetComponent<Room>();

        if (old != null)
        {
            Destroy(old);
        }

        Room r = null;
        switch (type)
        {
            case RoomType.Start:
                r =
                    obj.AddComponent<StartRoom>();
                break;

            case RoomType.Normal:
                r =
                    obj.AddComponent<NormalRoom>();
                break;
                //todo:room待拓展
            case RoomType.Boss:
                r =
                    obj.AddComponent<NormalRoom>();
                break;

            default:
                r =
                    obj.AddComponent<Room>();
                break;
        }
        rooms.Add(r);

        return r;
    }

    public void EnterRoom(Room room)
    {
        currentRoom = room;
        room.isVisited = true;

        Debug.Log("进入房间" + room.name);
    }

    public void AddDoor(Door door)
    {
        doorPositionMap.Add(door.transform.position, door);
    }

    public Door GetDoorByPosition(Vector2 position)
    {
        Door door;
        if (doorPositionMap.TryGetValue(position, out door))
        {
            return door;
        }
         return null;
    }

    public void RoomCleared(Room room)
    {
        room.isCleared = true;
        EnhancementManager.Instance.ShowEnhancement();
    }

    public void ResetRooms()
    {
        rooms.Clear();
        doorPositionMap.Clear();
        currentRoom = null;
    }

    public List<RoomSaveData> GetRoomSaveData()
    {
        List<RoomSaveData> list = new List<RoomSaveData>();

        foreach (var r in rooms)
        {
            list.Add(new RoomSaveData
            {
                id = r.roomID,
                cleared = r.isCleared,
                visited = r.isVisited
            });
        }

        return list;
    }

    public void ApplySaveData(DungeonSaveData data)
    {

        if (data == null) return;
        foreach (var save in data.rooms)
        {
            Room r = rooms.Find(x => x.roomID == save.id);

            if (r == null) continue;

            r.isCleared = save.cleared;
            r.isVisited = save.visited;
        }
    }

    public void SetCurrentRoom(int id)
    {
        foreach (var r in rooms)
        {
            if (r.roomID == id)
            {
                currentRoom = r;
                break;
            }
        }
    }
}
