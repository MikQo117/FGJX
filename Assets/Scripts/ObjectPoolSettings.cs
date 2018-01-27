using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolSetting", menuName = "ObjectPoolSettings", order = 1)]
public class ObjectPoolSettings : ScriptableObject
{
    [System.Serializable]
    public struct Values
    {
        public GameObject prefab;
        public string name;
        public int count;
    }

    public Values[] values;
}
