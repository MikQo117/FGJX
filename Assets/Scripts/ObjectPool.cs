using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolSettings settings;

    private List<GameObject>[] pools;
    private Transform[] poolParents;

    private void Awake()
    {
        pools = new List<GameObject>[settings.values.Length];
        poolParents = new Transform[pools.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
            poolParents[i] = new GameObject(settings.values[i].name + " pool").transform;
            for (int p = 0; p < settings.values[i].count; p++)
            {
                CreatePoolItem(i);
            }
        }
    }

    public GameObject GetItem(string name, Transform parent)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (settings.values[i].name == name)
            {
                for (int p = 0; p < pools[i].Count; p++)
                {
                    if (!pools[i][p].activeSelf)
                    {
                        pools[i][p].SetActive(true);
                        pools[i][p].transform.SetParent(parent);
                        return pools[i][p];
                    }
                }
                CreatePoolItem(i);
                pools[i][pools[i].Count - 1].SetActive(true);
                pools[i][pools[i].Count - 1].transform.SetParent(parent);
                return pools[i][pools[i].Count - 1];
            }
        }
        Debug.LogWarning("Item with name [" + name + "] was not found in pool.");
        return null;
    }

    public bool ReturnItem(GameObject item)
    {
        string[] nameSplit = item.name.Split(' ');
        string poolName = "";
        for (int i = 0; i < nameSplit.Length - 1; i++)
        {
            poolName += nameSplit[i];
        }
        return ReturnItem(item, poolName);
    }

    public bool ReturnItem(GameObject item, string poolName)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (poolName == settings.values[i].name)
            {
                item.SetActive(false);
                item.transform.SetParent(poolParents[i]);
                return true;
            }
        }
        return false;
    }

    private void CreatePoolItem(int index)
    {
        GameObject go = Instantiate(settings.values[index].prefab, Vector3.zero, Quaternion.identity);
        go.name = settings.values[index].name + " " + pools[index].Count;
        go.SetActive(false);
        go.transform.SetParent(poolParents[index]);
        pools[index].Add(go);
    }
}
