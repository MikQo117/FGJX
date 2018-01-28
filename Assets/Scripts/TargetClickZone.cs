using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClickZone : MonoBehaviour
{
    public int id;

    [SerializeField]
    [Range(0f,1f)]
    private float areaUpperLimit;
    [SerializeField]
    [Range(0f, 1f)]
    private float areaLowerLimit;

    [SerializeField]
    private string clickEffect;

    /*public delegate void TargetClickZoneEvent();
    public event TargetClickZoneEvent ValidClick;
    public event TargetClickZoneEvent InvalidClick;*/

    private TargetObjectManager targetObjectManager;
    private ObjectPool objectPool;

    private void Awake()
    {
        targetObjectManager = FindObjectOfType<TargetObjectManager>();
        objectPool = FindObjectOfType<ObjectPool>();
    }

    public bool Click(out TargetObject target)
    {
        target = null;
        for (int i = 0; i < targetObjectManager.targets.Count; i++)
        {
            float pos = targetObjectManager.targets[i].GetPos();
            if (pos >= areaLowerLimit)
            {
                if (pos < areaUpperLimit)
                {
                    target = targetObjectManager.targets[i];
                    if (target.id == id && target.GetIsClicked() != true)
                    {
                        target.SetIsClicked(true);
                        ClickEffect obj = objectPool.GetItem(clickEffect, null).GetComponent<ClickEffect>();
                        obj.transform.localPosition = Vector3.zero;
                        obj.Play(target.transform);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public float GetAreaUpperLimit()
    {
        return areaUpperLimit;
    }

    private void OnDrawGizmos()
    {
        if (!targetObjectManager)
            Awake();
        Color c = Gizmos.color;
        switch (id)
        {
            case 0:
                Gizmos.color = Color.green;
                break;

            case 1:
                Gizmos.color = Color.yellow;
                break;

            case 2:
                Gizmos.color = Color.red;
                break;

            default:
                Gizmos.color = Color.gray;
                break;
        }
        Gizmos.DrawLine(
            targetObjectManager.transform.position + Vector3.up * targetObjectManager.spawnDistance * areaLowerLimit + Vector3.right * (0.03f + 0.03f * id), 
            targetObjectManager.transform.position + Vector3.up * targetObjectManager.spawnDistance * areaUpperLimit + Vector3.right * (0.03f + 0.03f * id));
        Gizmos.color = c;
    }
}
