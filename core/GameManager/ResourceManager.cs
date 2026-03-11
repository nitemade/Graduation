using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager
    : Singleton<ResourceManager>
{
    public void InstantiateAsync(
        String address,
        Vector3 pos,
        Quaternion rot,
        Transform parent,
        Action<GameObject> cb
        )
    {
        Addressables.InstantiateAsync(
            address,
            pos,
            rot,
            parent).Completed += (h) =>
            {
                cb?.Invoke(h.Result);
            };
    }

    public void Release(GameObject obj)
    {
        Addressables.ReleaseInstance(obj);
    }
}