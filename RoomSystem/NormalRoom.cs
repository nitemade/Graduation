using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoom : Room
{
    private GameObject enemyPrefab;

    private bool spawned = false;

    private int enemyCount = 0;

    private HashSet<Vector3Int> spawnPoints = new HashSet<Vector3Int>();

    public int minEnemyCs = 1;
    public int maxEnemyCs = 3;

    private void Awake()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Orc");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawned)
        {
            spawned = true;

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
            int eY = Random.Range(roomRect.y + 2, roomRect.y + roomRect.height - 1);

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
    public override void EnemyDead()
    {
        enemyCount--;
        Debug.Log("剩余敌人: " + enemyCount);
        if (enemyCount <= 0)
        {
            OpenDoors();
        }
    }
}
