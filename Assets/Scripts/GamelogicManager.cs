using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamelogicManager : MonoBehaviour
{
    public int score = 0;
    public TargetClickZone[] zones;

    private void Update()
    {
        ClickZone(KeyCode.A, zones[0]);
        ClickZone(KeyCode.S, zones[1]);
        ClickZone(KeyCode.D, zones[2]);
    }

    private void ClickZone(KeyCode key, TargetClickZone zone)
    {
        if (Input.GetKeyDown(key))
        {
            TargetObject target;
            if (zone.Click(out target))
            {
                score += target.scoreValue;
            }
            else
            {
                score = 0;
            }
        }
    }
}
