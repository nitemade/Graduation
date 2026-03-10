using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public Room currentRoom;

    private Dictionary<Vector2, Door> doorPositionMap = new Dictionary<Vector2, Door>();

    private void Awake()
    {
        doorPositionMap.Clear();
        instance = this;
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

        return r;
    }

    public void EnterRoom(Room room)
    {
        currentRoom = room;

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

    public void RoomCleared()
    {
        EnhancementManager.Instance.ShowEnhancement();
    }
}
