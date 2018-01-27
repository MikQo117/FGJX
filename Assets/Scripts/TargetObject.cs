using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public int id;
    public Color[] idColors;
    public int scoreValue;

    private bool isMoving = false;
    private bool isInitilaized = false;
    private bool isClicked = false;

    private TargetObjectManager targetObjectManager;
    private ObjectPool poolManager;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float moveLerp = 0f;
    private SpriteRenderer sp;

    public delegate void TargetObjectDelegate(object sender);
    public event TargetObjectDelegate EndReached;

    public void BeginMove(Vector3 start, Vector3 end)
    {
        if (!isInitilaized)
            Initialize();
        isMoving = true;
        isClicked = false;
        moveLerp = 0f;
        moveStart = start;
        moveEnd = end;
        transform.position = moveStart;
        id = Random.Range(0, 3);
        // Early method for distinguishing different ids
        sp = GetComponent<SpriteRenderer>();
        switch (id)
        {
            case 0:
                sp.color = Color.green;
                break;

            case 1:
                sp.color = Color.yellow;
                break;

            case 2:
                sp.color = Color.red;
                break;

            default:
                sp.color = Color.gray;
                break;
        }
    }

    public float GetPos()
    {
        return 1f - moveLerp;
    }

    public bool GetIsClicked()
    {
        return isClicked;
    }

    public void SetIsClicked(bool value)
    {
        if (value)
        {
            sp.color = Color.gray;
            TargetObject[] signalTargets = FindClosestTargets(100f, 0.6f, 0.1f, 3);
            for (int i = 0; i < signalTargets.Length; i++)
            {
                SignalEffect signalEffect = poolManager.GetItem("signal", transform.parent).GetComponent<SignalEffect>();
                signalEffect.BeginMove(transform.position, signalTargets[i]);
            } 
        }
        isClicked = value;
    }

    private TargetObject[] FindClosestTargets(float maxDistance, float minDistance, float minCircularDistance, int maxAmount)
    {
        List<TargetObject> foundObj = new List<TargetObject>();
        List<float> foundDist = new List<float>();
        int longestDistIndex = -1;
        foreach (TargetObject t in targetObjectManager.targets)
        {
            if (t.GetPos() > GetPos() + minCircularDistance)
            {
                float dist = Vector3.Distance(transform.position, t.transform.position);
                if (dist >= minDistance && dist <= maxDistance)
                {
                    if (foundObj.Count == 0)
                    {
                        foundObj.Add(t);
                        foundDist.Add(dist);
                        longestDistIndex = foundObj.Count - 1;
                    }
                    else if(foundObj.Count < maxAmount)
                    {
                        foundObj.Add(t);
                        foundDist.Add(dist);
                        if (dist > foundDist[longestDistIndex])
                        {
                            longestDistIndex = foundObj.Count - 1;
                        }
                    }
                    else
                    {
                        if (dist < foundDist[longestDistIndex])
                        {
                            foundObj.RemoveAt(longestDistIndex);
                            foundDist.RemoveAt(longestDistIndex);
                            foundObj.Add(t);
                            foundDist.Add(dist);
                            longestDistIndex = foundObj.Count - 1;
                        }
                    }
                }
            }
        }
        return foundObj.ToArray();
    }

    private void Update()
    {
        if (isMoving)
        {
            moveLerp += targetObjectManager.targetSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(moveStart, moveEnd, moveLerp);
            if (moveLerp >= 1f)
            {
                isMoving = false;
                if (EndReached != null)
                    EndReached.Invoke(this);
            }
        }
    }

    private void Initialize()
    {
        targetObjectManager = FindObjectOfType<TargetObjectManager>();
        poolManager = FindObjectOfType<ObjectPool>();
        isInitilaized = true;
    }
}
