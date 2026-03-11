using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Queue<GameObject>>
        pool = new();

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
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.transform.parent = parent;

            cb?.Invoke(obj);

            return;
        }

        ResourceManager.Instance.InstantiateAsync(
            address,
            pos,
            rot,
            parent,
            cb);
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
}