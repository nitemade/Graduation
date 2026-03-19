using Cinemachine;
using UnityEngine;

public class StartRoom : Room
{
    public CinemachineVirtualCamera vcam;

    private void Start()
    {
        SpawnPlayer();
        transform.name = "StartRoom";

        isVisited = true;
        isCleared = true;
    }

    private void SpawnPlayer()
    {
        if (vcam == null)
        {
            vcam = FindObjectOfType<CinemachineVirtualCamera>();
        }

        Vector3 playerPos = new Vector3(
            roomRect.x + roomRect.width / 2,
            roomRect.y + roomRect.height / 2,
            0);

        PoolManager.Instance.Spawn(AddressConst.SOLDIER, playerPos, Quaternion.identity, null,
            (playerOjb) =>
            {
                playerOjb.name = "Player";
                playerOjb.layer = LayerMask.NameToLayer("Players");
                playerOjb.tag = "Player";

                vcam.Follow = playerOjb.transform;
                if (isCleared && isVisited)
                    PlayerManager.Instance.RegisterPlayer(playerOjb.GetComponent<PlayerController>());
            });
    }
}
