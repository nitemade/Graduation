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
            Gizmos.color = Color.green;
            RectInt r = leaf.rect;

            Vector3 pos = new Vector3(
                r.x + r.width / 2f,
                r.y + r.height / 2f,
                0
            );

            Vector3 size = new Vector3(r.width, r.height, 1);

            Gizmos.DrawWireCube(pos, size);

            RectInt room = leaf.room;

            Gizmos.color = Color.yellow;
            Vector3 roomPos = new Vector3(
                room.x + room.width / 2f,
                room.y + room.height / 2f,
                0);

            Vector3 roomsize = new Vector3(room.width, room.height, 1);

            Gizmos.DrawWireCube(roomPos, roomsize);
        }
    }
}
