using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Transform[] spawnPoints;
    private Door[] doors;
    public GameObject enemyPrefab;

    private int enemyCount = 0;
    private bool enemiesSpawned = false;

    private void Awake()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        spawnPoints = System.Array.FindAll(allChildren,t => t.CompareTag("SpawnPoint"));
        doors = GetComponentsInChildren<Door>();
    }

    private void Start()
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
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
        foreach(Transform spawn in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
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
        foreach (Door door in doors)
        {
                door.Close();
        }
    }
    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
    }
}
