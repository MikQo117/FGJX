using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectManager : MonoBehaviour
{
    public List<TargetObject> targets = new List<TargetObject>();
    public float targetSpeed;
    public float spawnInterval;
    public float spawnDistance;
    public float speedUpPercentDuringMinute;

    [SerializeField]
    private string targetPrefab;
    private float spawnTimer = 0f;
    private ObjectPool objectPool;

    public delegate void TargetObjectManagerDelegate(object sender);
    public event TargetObjectManagerDelegate FailedToClickInTime;

    private void Awake()
    {
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            // Base time
            spawnTimer = (spawnInterval + spawnTimer) / (1 + (Time.timeSinceLevelLoad / 60) * (speedUpPercentDuringMinute / 100));
            // Logic for determining spawn position. Currently completely random.
            Vector3 spawnPos = spawnDistance * Random.insideUnitCircle.normalized;
            TargetObject target = objectPool.GetItem(targetPrefab, transform).GetComponent<TargetObject>();
            targets.Add(target);
            target.BeginMove(spawnPos, transform.position);
            target.EndReached += TargetPathEndReached;        
        }
    }

    private void TargetPathEndReached(object sender)
    {
        TargetObject obj = (TargetObject)sender;
        if (!obj.GetIsClicked())
        {
            FailedToClickInTime.Invoke(this);
        }
        obj.EndReached -= TargetPathEndReached;
        objectPool.ReturnItem(obj.gameObject);
        targets.RemoveAt(0); // Removes at 0 as it is expected that oldest object is first to arrive to end
    }

    private void OnDrawGizmos()
    {
        Color c = Gizmos.color;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * spawnDistance);
        Gizmos.color = c;
    }
}
