using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Room : MonoBehaviour
{
    private HashSet<Vector3Int> spawnPoints = new HashSet<Vector3Int>();
    private HashSet<Vector2> doorPoint = new HashSet<Vector2>();
    private RoomManager roomManager;
    LayerMask doorMask;

    public GameObject enemyPrefab;

    private RectInt roomRect;
    [SerializeField] private int maxEnemyCs = 3;
    [SerializeField] private int minEnemyCs = 1;

    private int enemyCount = 0;
    private bool enemiesSpawned = false;


    private void Awake()
    {
        doorMask = LayerMask.GetMask("Door");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Orc");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !enemiesSpawned)
        {
            enemiesSpawned = true;

            RoomManager.instance.EnterRoom(this);

            SpawnEnemies();

            CloseDoors();

        }
    }


    private void SpawnEnemies()
    {
        int eCounts = Random.Range(minEnemyCs, maxEnemyCs);
        while (eCounts > 0)
        {
            int eX = Random.Range(roomRect.x + 2, roomRect.x + roomRect.width - 1);
            int eY = Random.Range(roomRect.y + 2, roomRect.y + roomRect.height -1);

            Vector3Int v = new Vector3Int(eX, eY, 0);

            if (spawnPoints.Add(v))
            {
                eCounts--;
            }
        }
        foreach (var spawn in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawn, Quaternion.identity);
            enemy.transform.parent = transform;
            AIStateMachine aIState = enemy.GetComponent<AIStateMachine>();
            aIState.Init(this);
            enemyCount++;
        }
        Debug.Log("生成敌人数量: " + enemyCount);
    }
    public void EnemyDead()
    {
        enemyCount--;
        Debug.Log("剩余敌人: " + enemyCount);
        if (enemyCount <= 0)
        {
            OpenDoors();
        }
    }
    private void CloseDoors()
    {
        foreach (var p in doorPoint)
        {
            Door door = GetDoorAt(p);

            if (door != null)
            {
                door.Close();
            }
        }
    }
    private void OpenDoors()
    {
        foreach (var p in doorPoint)
        {
            Door door = GetDoorAt(p);

            if (door != null)
            {
                door.Open();
            }
        }
    }

    Door GetDoorAt(Vector2 pos)
    {
        return roomManager.GetDoorByPosition(pos);

    }
    public void SetRoomRect(RectInt room)
    {
        roomRect = room;
    }

    public void SetDoorPoints(Vector2 door,RoomManager roomManager)
    {
        this.roomManager = roomManager;
        doorPoint.Add(door);
    }
}
