using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct LeafPrefabInfo
{
    public readonly string path;
    public readonly string name;

    public LeafPrefabInfo(string path, string name)
    {
        this.path = path;
        this.name = name;
    }

    public GameObject prefab => Resources.Load<GameObject>(path);
}