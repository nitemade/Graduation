using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public Room currentRoom;

    private void Awake()
    {
        instance = this;
    }

    public void EnterRoom(Room room)
    {
        currentRoom = room;

        Debug.Log("进入房间" + room.name);
    }
}
