using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager>
{
    public RectTransform mapRoot;

    Dictionary<int, MinimapRoomUI> map =
        new Dictionary<int, MinimapRoomUI>();
    Dictionary<int, List<GameObject>> doorMap =
    new Dictionary<int, List<GameObject>>();

    float scale = 1.5f;

    public void RegisterRoom(Room room, RectInt rect, RoomType type, HashSet<Vector2Int> doors)
    {
        Vector3 pos = Vector3.zero;

        PoolManager.Instance.Spawn(
            AddressConst.MINIMAP_ROOM,
            pos,
            Quaternion.identity,
            mapRoot,
            (obj) =>
            {
                var ui = obj.GetComponent<MinimapRoomUI>();

                ui.Init(type);

                RectTransform rt = ui.GetComponent<RectTransform>();

                rt.anchoredPosition =
                    new Vector2(
                        rect.x * scale,
                        rect.y * scale
                    );

                rt.sizeDelta =
                    new Vector2(
                        rect.width * scale,
                        rect.height * scale
                    );

                if (!room.isVisited)
                    ui.gameObject.SetActive(false);

                map.Add(room.roomID, ui);
            });

        doorMap[room.roomID] =
    new List<GameObject>();

        foreach (var d in doors)
        {
            SpawnDoor(room, rect, d);
        }
    }

    void SpawnDoor(
    Room room,
    RectInt rect,
    Vector2 doorPos)
    {
        PoolManager.Instance.Spawn(
            AddressConst.MINIMAP_DOOR,
            Vector3.zero,
            Quaternion.identity,
            mapRoot,
            (obj) =>
            {
                RectTransform rt =
                    obj.GetComponent<RectTransform>();

                rt.anchoredPosition =
                    new Vector2(
                        doorPos.x * scale,
                        doorPos.y * scale
                    );

                obj.SetActive(false);

                doorMap[room.roomID].Add(obj);
            });
    }

    public void ShowRoom(Room room)
    {
        if (!map.ContainsKey(room.roomID))
            return;

        var ui = map[room.roomID];

        ui.gameObject.SetActive(true);


        if (doorMap.ContainsKey(room.roomID))
        {
            foreach (var d in doorMap[room.roomID])
                d.SetActive(true);
        }
    }
}