using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : Room
{
    private GameObject playerPrefab;
    public CinemachineVirtualCamera vcam;

    public override void Init(RectInt room, RoomManager manager)
    {
        base.Init(room, manager);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (vcam == null)
        {
            vcam = FindObjectOfType<CinemachineVirtualCamera>();
        }

        playerPrefab = Resources.Load<GameObject>("Prefabs/Players/Soldier");

        GameObject player = Instantiate(playerPrefab);

        player.transform.position = new Vector3(
            roomRect.x + roomRect.width / 2, 
            roomRect.y + roomRect.height / 2, 
            0);

        player.layer = LayerMask.NameToLayer("Players");
        player.tag = "Player";

        vcam.Follow = player.transform;
    }
}
