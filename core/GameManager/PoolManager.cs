using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Queue<GameObject>> pool = new();

    public void Spawn(
        string address,
        Vector3 pos,
        Quaternion rot,
        Transform parent,
        System.Action<GameObject> cb)
    {
        if (pool.ContainsKey(address) &&
            pool[address].Count > 0)
        {
            var obj = pool[address].Dequeue();

            obj.SetActive(true);
            obj.transform.SetParent(parent, true);

            obj.transform.position = pos;

            obj.transform.rotation = rot;


            cb?.Invoke(obj);

            return;
        }

        ResourceManager.Instance.InstantiateAsync(
        address,
        pos,
        rot,
        parent,
        (obj) =>
        {

            cb?.Invoke(obj);
        });
    }

    public void Despawn(
        string address,
        GameObject obj)
    {
        obj.SetActive(false);

        if (!pool.ContainsKey(address))
            pool[address] = new();

        pool[address].Enqueue(obj);
    }

    public void ReleaseAll(string address)
    {
        if (!pool.ContainsKey(address))
            return;

        foreach (var obj in pool[address])
        {
            if (obj != null)
                obj.SetActive(false);
        }

        pool[address].Clear();
    }

    public void ReleaseAllActive(string address)
    {

        GameObject[] objs =
            GameObject.FindObjectsOfType<GameObject>();

        int count = 0;

        foreach (var obj in objs)
        {
            if (!obj.activeInHierarchy)
                continue;

            if (obj.name.Contains(address))
            {
                count++;

                Despawn(address, obj);
            }
        }
    }
}