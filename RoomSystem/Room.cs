using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Room : MonoBehaviour
{
    protected HashSet<Vector2> doorPoint = new HashSet<Vector2>();
    protected RectInt roomRect;

    public int roomID;

    public bool isCleared;
    public bool isVisited;

    public virtual void Init(RectInt room, int id)
    {
        roomRect = room;
        roomID = id;
        isCleared = false;
        isVisited = false;
    }

    public virtual void Init(RectInt room, int id, RoomSaveData data)
    {
        roomRect = room;
        roomID = id;
        isCleared = data.cleared;
        isVisited = data.visited;
    }


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
        return RoomManager.Instance.GetDoorByPosition(pos);

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
