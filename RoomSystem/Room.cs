using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Room : MonoBehaviour
{
    protected HashSet<Vector2> doorPoint = new HashSet<Vector2>();
    protected RoomManager roomManager;
    protected RectInt roomRect;

    protected void CloseDoors()
    {
        foreach (var p in doorPoint)
        {
            Door door = GetDoor(p);

            if (door != null)
            {
                door.Close();
            }
        }
    }
    protected void OpenDoors()
    {
        foreach (var p in doorPoint)
        {
            Door door = GetDoor(p);

            if (door != null)
            {
                door.Open();
            }
        }
    }

    protected Door GetDoor(Vector2 pos)
    {
        return roomManager.GetDoorByPosition(pos);

    }
    public virtual void Init(RectInt room,RoomManager manager)
    {
        roomRect = room;
        roomManager = manager;
    }

    public virtual void EnemyDead()
    {
        throw new NotImplementedException("请在子类中实现 EnemyDead 方法");
    }

    public void AddDoor(Vector2 door)
    {
        doorPoint.Add(door);
    }
}
