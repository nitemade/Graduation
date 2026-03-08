using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;

    public int minLeafSize = 10;

    BSPNode root;

    List<BSPNode> leaves = new List<BSPNode>();
    List<Edge> edges = new List<Edge>();
    List<Vector2Int> corridors = new List<Vector2Int>();

    private void Start()
    {
        GenerateBSP();
    }

    private void GenerateBSP()
    {
        root = new BSPNode(new RectInt(0, 0, mapWidth, mapHeight));

        SplitNode(root);

        GetLeaves(root);

        CreateRooms();

        List<RectInt> roomList = new List<RectInt>();

        foreach (var leaf in leaves)
        {
            roomList.Add(leaf.room);
        }

        edges = GenerateMST(roomList);

        CreateCorridors();
        Debug.Log("Leaf count:" + leaves.Count);
    }

    private void CreateRooms()
    {
        foreach (var leaf in leaves)
        {
            int padding = 2;
            int roomWidth = Random.Range(
                leaf.rect.width / 2,
                leaf.rect.width - padding);

            int roomHeight = Random.Range(
                leaf.rect.height / 2,
                leaf.rect.height - padding);

            int roomx = Random.Range(
                leaf.rect.x + 1,
                leaf.rect.x + leaf.rect.width - roomWidth - 1);

            int roomy = Random.Range(
                leaf.rect.y + 1,
                leaf.rect.y + leaf.rect.height - roomHeight - 1);

            leaf.room = new RectInt(roomx, roomy, roomWidth, roomHeight);
        }
    }

    private void CreateCorridor(Vector2Int a, Vector2Int b)
    {
        bool horizontalFirst = Random.value > 0.5f;
        if (horizontalFirst)
        {
            Vector2Int mid = new Vector2Int(b.x, a.y);

            DrawLine(a, mid);
            DrawLine(mid, b);
        }
        else
        {
            Vector2Int mid = new Vector2Int(a.x, b.y);

            DrawLine(a, mid);
            DrawLine(mid, b);
        }
    }
    private void CreateCorridors()
    {
        corridors.Clear();

        foreach (var edge in edges)
        {
            CreateCorridor(edge.a,edge.b);
        }
    }

    /// <summary>
    /// 二叉空间分割（BSP树）
    /// </summary>
    /// <param name="node"></param>
    private void SplitNode(BSPNode node)
    {
        if (node.rect.width < minLeafSize *2 && node.rect.height < minLeafSize * 2)
        {
            return;
        }

        bool splitHorizontally;
        if (node.rect.width > node.rect.height)
        {
            splitHorizontally = false;
        }
        else
        {
            splitHorizontally = true;
        }

        if (splitHorizontally)
        {
            int split = Random.Range(minLeafSize, node.rect.height - minLeafSize);

            RectInt rect1 = new RectInt(
                node.rect.x,
                node.rect.y,
                node.rect.width,
                split
                );

            RectInt rect2 = new RectInt(
                node.rect.x,
                node.rect.y + split,
                node.rect.width,
                node.rect.height - split);

            node.left = new BSPNode(rect1);
            node.right = new BSPNode(rect2);
        }
        else
        {
            int split = Random.Range(minLeafSize, node.rect.width - minLeafSize);

            RectInt rect1 = new RectInt(
                node.rect.x,
                node.rect.y,
                split,
                node.rect.height);

            RectInt rect2 = new RectInt(
                node.rect.x + split,
                node.rect.y,
                node.rect.width - split,
                node.rect.height);

            node.left = new BSPNode(rect1);
            node.right = new BSPNode(rect2);
        }        

        SplitNode(node.left);
        SplitNode(node.right);

    }

    private void GetLeaves(BSPNode node)
    {
        if (node.IsLeaf())
        {
            leaves.Add(node);
            return;
        }

        if (node.left != null)
        {
            GetLeaves(node.left);
        }

        if (node.right != null)
        {
            GetLeaves(node.right);
        }
    }


    private void OnDrawGizmos()
    {
        if (leaves == null) return;



        foreach (var leaf in leaves)
        {
            //绘制空间
            Gizmos.color = Color.green;
            RectInt r = leaf.rect;

            Vector3 pos = new Vector3(
                r.x + r.width / 2f,
                r.y + r.height / 2f,
                0
            );

            Vector3 size = new Vector3(r.width, r.height, 1);

            Gizmos.DrawWireCube(pos, size);


            //绘制房间
            RectInt room = leaf.room;

            Gizmos.color = Color.yellow;
            Vector3 roomPos = new Vector3(
                room.x + room.width / 2f,
                room.y + room.height / 2f,
                0);

            Vector3 roomsize = new Vector3(room.width, room.height, 1);

            Gizmos.DrawWireCube(roomPos, roomsize);

        }

        //绘制边
        Gizmos.color = Color.red;

        foreach (var edge in edges)
        {
            Gizmos.DrawLine(
                new Vector3(edge.a.x, edge.a.y, 0),
                new Vector3(edge.b.x, edge.b.y, 0));
        }

        //绘制走廊
        Gizmos.color = Color.white;

        foreach (var corridor in corridors)
        {
            Gizmos.DrawCube(
                new Vector3(corridor.x, corridor.y, 0),
                Vector3.one * 0.5f);
                
        }
    }

    void DrawLine(Vector2Int form, Vector2Int to)
    {
        Vector2Int pos = form;

        while (pos != to)
        {
            corridors.Add(pos);

            if (pos.x < to.x) pos.x++;
            else if (pos.x > to.x) pos.x--;
            else if (pos.y < to.y) pos.y++;
            else if (pos.y > to.y) pos.y--;
        }

        corridors.Add(to);
    }

    Vector2Int GetRoomCenter(RectInt room)
    {
        return new Vector2Int(
            room.x + room.width / 2,
            room.y + room.height / 2);
    }


    /// <summary>
    /// 生成最小生成树(Prim)
    /// </summary>
    /// <param name="rooms"></param>
    /// <returns></returns>
    List<Edge> GenerateMST(List<RectInt> rooms)
    {
        List<Edge> mst = new List<Edge>();

        List<Vector2Int>  notes = new List<Vector2Int>();

        foreach (var room in rooms)
        {
            notes.Add(GetRoomCenter(room));
        }

        List<Vector2Int> connected = new List<Vector2Int>();
        List<Vector2Int> remaining = new List<Vector2Int>(notes);

        connected.Add(remaining[0]);
        remaining.RemoveAt(0);

        while(remaining.Count > 0)
        {
            Edge shortest = null;

            foreach (var c in connected)
            {
                foreach (var r in remaining)
                {
                    Edge e = new Edge(c, r);
                    if (shortest == null || shortest.distance > e.distance)
                    {
                        shortest = e;
                    }
                }
            }

            mst.Add(shortest);

            connected.Add(shortest.b);
            remaining.Remove(shortest.b);
        }

        return mst;
    }
}
