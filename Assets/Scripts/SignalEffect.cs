using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEffect : MonoBehaviour
{
    [SerializeField]
    private float moveEndpointOffset;

    private TargetObjectManager tm;
    private GamelogicManager gm;
    private ObjectPool op;
    private LineRenderer lr;

    private TargetObject targetObject;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float targetStartGoalDist;
    private float moveLerp = 0f;
    private bool isMoving = false;
    private bool isInitilaized = false;
    private TargetClickZone zone = null;

    public void BeginMove(Vector3 start, TargetObject target)
    {
        if (!isInitilaized)
            Initialize();
 
        for (int i = 0; i < gm.zones.Length; i++)
        {
            if (gm.zones[i].id == target.id)
            {
                zone = gm.zones[i];
                targetStartGoalDist = target.GetPos() - (zone.GetAreaUpperLimit() + moveEndpointOffset);
            }
        }

        targetObject = target;
        isMoving = true;
        moveStart = start;
        moveEnd = (zone.GetAreaUpperLimit() + moveEndpointOffset) * tm.spawnDistance * target.transform.position.normalized;
    }

    private void Update()
    {
        if (isMoving)
        {
            moveLerp = 1f - (targetObject.GetPos() - (zone.GetAreaUpperLimit() + moveEndpointOffset)) / targetStartGoalDist;
            transform.position = Vector3.Lerp(moveStart, moveEnd, moveLerp);
            lr.SetPositions(new Vector3[] { transform.position, targetObject.transform.position });

            if (moveLerp >= 1f)
            {
                isMoving = false;
                op.ReturnItem(gameObject);
            }
        }
    }

    private void Initialize()
    {
        gm = FindObjectOfType<GamelogicManager>();
        tm = FindObjectOfType<TargetObjectManager>();
        op = FindObjectOfType<ObjectPool>();
        lr = GetComponent<LineRenderer>();
        isInitilaized = true;
    }

    private void OnDrawGizmos()
    {
        if (isMoving)
        {
            Color c = Gizmos.color;
            Gizmos.DrawLine(moveStart, moveEnd);
            Gizmos.color = c;
        }
    }
}
