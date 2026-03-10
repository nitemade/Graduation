using Cinemachine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;

    public int minLeafSize = 10;

    public Tilemap floorTilemap;
    public TileBase floorTile;
    public Tilemap wallTilemap;
    public TileBase wallTile;

    private GameObject roomPrefab;
    private GameObject doorPrefab;
    private GameObject playerPrefab;

    public CinemachineVirtualCamera virtualCamera;
    private RoomManager roomManager;

    public int seed = 12345;
    public bool randomSeed = true;

    BSPNode root;

    List<BSPNode> leaves = new List<BSPNode>();
    List<Edge> edges = new List<Edge>();

    HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
    HashSet<Vector2Int> wall = new HashSet<Vector2Int>();
    HashSet<Vector2Int> doors  = new HashSet<Vector2Int>();


    Dictionary<BSPNode, int> leafIndex = new Dictionary<BSPNode, int>();

    private void Awake()
    {
        roomManager = GetComponent<RoomManager>();

        roomPrefab = Resources.Load<GameObject>("Prefabs/Room/Room");
        doorPrefab = Resources.Load<GameObject>("Prefabs/Room/Door");
        playerPrefab = Resources.Load<GameObject>("Prefabs/Players/Soldier");
    }

    private void Start()
    {
        if (randomSeed)
            seed = Random.Range(0 , 999999);
        Random.InitState(seed);

        GenerateBSP();

        CreateRoomObjects();

        if (AstarPath.active != null)
        {
            GridGraph gridGraph = AstarPath.active.data.gridGraph;
            gridGraph.SetDimensions(mapWidth, mapHeight,1);
            gridGraph.center = new Vector3(mapWidth / 2f, mapHeight / 2f, 0);
            AstarPath.active.Scan();
            Debug.Log("网格创建成功");
        }

        BuildTiles();
    }

    private void BuildTiles()
    {
        floorTilemap.ClearAllTiles();

        CollectRooms();
        CollectCorridors();

        GenerateWalls();

        foreach (var p in floor)
        {
            floorTilemap.SetTile(
                new Vector3Int(p.x,p.y,0),
                floorTile);

        }

        foreach (var p in wall)
        {
            wallTilemap.SetTile(
                new Vector3Int(p.x, p.y, 0),
                wallTile);
        }
    }

    private void GenerateBSP()
    {
        root = new BSPNode(new RectInt(0, 0, mapWidth, mapHeight));

        SplitNode(root);

        GetLeaves(root);

        CreateRooms();

        IndexLeaves();

        AssignStartRoom();

        AssignBossRoom();

        AssignSpecialRooms();


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

    private void CreateRoomObjects()
    {
        foreach (var leaf in leaves)
        {

            RoomInit(leaf);

            RectInt r = leaf.room;

            GameObject obj = Instantiate(roomPrefab);

            obj.name = "Room";

            obj.transform.position =
                new Vector3(
                    r.x + r.width / 2f,
                    r.y + r.height / 2f,
                    0
                );

            BoxCollider2D col = obj.GetComponent<BoxCollider2D>();

            col.size = new Vector2Int(r.width, r.height);

            Room rc =  obj.GetComponent<Room>();

            rc.SetRoomRect(leaf.room);


            if (leaf.doors.Count == 0)
                continue;

            foreach (var door in leaf.doors)
            {
                GameObject obj2 = Instantiate(doorPrefab);

                obj2.name = "Door";

                obj2.layer = LayerMask.NameToLayer("Door");
                obj2.tag = "Door";

                obj2.transform.position =
                    new Vector3(
                        door.x,
                        door.y,
                        0
                    );

                roomManager.AddDoor(obj2.GetComponent<Door>());

                obj2.SetActive(false);


                rc.SetDoorPoints(door,roomManager);
            }
        }
    }

    private void RoomInit(BSPNode leaf)
    {
        if (leaf.roomType == RoomType.Start)
        {
            if (virtualCamera == null)
            {
                // 如果没有提前引用，就动态查找场景中的虚拟相机
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }


            GameObject player = Instantiate(playerPrefab);

            player.transform.position =
                new Vector3(
                    leaf.room.x + leaf.room.width / 2f,
                    leaf.room.y + leaf.room.height / 2f,
                    0
                );
            player.layer = LayerMask.NameToLayer("Players");
            player.tag = "Player";

            if (virtualCamera != null && player != null)
            {
                // 关键：设置 Follow 目标为玩家的 Transform
                virtualCamera.Follow = player.transform;
                Debug.Log("虚拟相机已跟随玩家");
            }
        }
    }

    void IndexLeaves()
    {
        leafIndex.Clear();
        for (int i = 0; i < leaves.Count; i++)
        {
            leafIndex.Add(leaves[i], i);
        }
    }

    void AssignStartRoom()
    {
        BSPNode start = leaves[0];

        foreach(var leaf in leaves)
        {
            if (leaf.room.x < start.room.x)
                start = leaf;
        }

        start.roomType = RoomType.Start;
    }

    private void AssignBossRoom()
    {
        BSPNode start = null;
        foreach (var leaf in leaves)
        {
            if (leaf.roomType == RoomType.Start)
                start = leaf;
        }

        BSPNode farthest = start;
        float maxDist = 0;

        Vector2Int startc = GetRoomCenter(start.room);

        foreach (var leaf in leaves)
        {
            Vector2Int c = GetRoomCenter(leaf.room);

            float d = Vector2Int.Distance(startc, c);

            if (d > maxDist)
            {
                maxDist = d;
                farthest = leaf;
            }
        }

        farthest.roomType = RoomType.Boss;
    }    

    void AssignSpecialRooms()
    {
        foreach (var leaf in leaves)
        {
            if (leaf.roomType != RoomType.Normal)
                continue;

            float r = Random.value;

            if (r < 0.05f)
                leaf.roomType = RoomType.Shop;
            else if (r < 0.1f)
                leaf.roomType = RoomType.Treasure;
        }
    }



    private void CreateCorridor(Vector2Int a, Vector2Int b, bool isHorizontal)
    {
        Vector2Int m = new Vector2Int((a.x + b.x) / 2, (a.y + b.y) / 2);
        if (isHorizontal)
        {
            Vector2Int mida = new Vector2Int(m.x, a.y);

            DrawLine(a, mida);
            DrawLine(mida, m);

            Vector2Int midb = new Vector2Int(m.x, b.y);
            DrawLine(midb, b);
            DrawLine(m, midb);
        }
        else
        {
            Vector2Int mida = new Vector2Int(a.x, m.y);

            DrawLine(a, mida);
            DrawLine(mida, m);

            Vector2Int midb = new Vector2Int(b.x, m.y);
            DrawLine(midb, b);
            DrawLine(m, midb);
        }
    }
    private void CreateCorridors()
    {
        corridors.Clear();

        foreach (var edge in edges)
        {
            CreateCorridor(edge.aP,edge.bP,edge.isHorizontal);
        }
    }

    void GenerateWalls()
    {
        wall.Clear();

        foreach (var f in floor)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2Int p = new Vector2Int(f.x + x, f.y + y);

                    if (!floor.Contains(p))
                    {
                        wall.Add(p);
                    }
                }
            }
        }
    }


    void CollectRooms()
    {
        floor.Clear();
        foreach (var leaf in leaves)
        {
            RectInt r = leaf.room;

            for (int x = r.x;x < r.xMax; x++)
            {
                for (int y = r.y; y < r.yMax; y++)
                {
                    floor.Add(new Vector2Int(x, y));
                }
            }

        }

    }

    void CollectCorridors()
    {
        foreach (var c in corridors)
        {
            floor.Add(c);
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

            Gizmos.color = GetRoomColor(leaf.roomType);
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

    Color GetRoomColor(RoomType t)
    {
        switch (t)
        {
            case RoomType.Start:
                return Color.blue;
            case RoomType.Normal:
                return Color.white;
            case RoomType.Boss:
                return Color.red;
            case RoomType.Shop:
                return Color.green;
            case RoomType.Treasure:
                return Color.cyan;
            default:
                return Color.yellow;
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

        List<int> connected = new List<int>();
        List<int> remaining = new List<int>(notes.Count);

        for (int i = 0; i < notes.Count; i++)
        {
            remaining.Add(i);
        }

        connected.Add(remaining[0]);
        remaining.RemoveAt(0);

        while(remaining.Count > 0)
        {
            Edge shortest = null;
            int rc = 0;
            int cc = 0;
            foreach (var c in connected)
            {
                foreach (var r in remaining)
                {
                    Edge e = new Edge(notes[c], notes[r], rooms[c], rooms[r]);
                    if (shortest == null || shortest.distance > e.distance)
                    {
                        shortest = e;
                        rc = r;
                        cc = c;
                    }
                }
            }

            leaves[cc].doors.Add(shortest.aP);
            leaves[rc].doors.Add(shortest.bP);
            doors.Add(shortest.aP);
            doors.Add(shortest.bP);

            mst.Add(shortest);

            connected.Add(rc);
            remaining.Remove(rc);
        }

        return mst;
    }
}
