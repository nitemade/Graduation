using UnityEngine;
using UnityEngine.UI;

public class MinimapRoomUI : MonoBehaviour
{
    public Image image;

    RoomType room;

    public void Init(RoomType r)
    {
        room = r;

        switch (r)
        {
            case RoomType.Start:
                image.color = Color.blue;
                break;

            case RoomType.Boss:
                image.color = Color.red;
                break;

            case RoomType.Shop:
                image.color = Color.green;
                break;

            default:
                image.color = Color.white;
                break;
        }
    }
}