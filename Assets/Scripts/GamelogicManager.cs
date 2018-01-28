using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamelogicManager : MonoBehaviour
{
    public int score = 0;
    public TargetClickZone[] zones;
    public TargetObjectManager tmManager;

    [SerializeField]
    private AudioSource failSound;
    [SerializeField]
    private AudioSource successSound;

    private void Start()
    {
        tmManager.FailedToClickInTime += FailedToClickInTime;
    }

    private void Update()
    {
        ClickZone(KeyCode.A, zones[0]);
        ClickZone(KeyCode.S, zones[1]);
        ClickZone(KeyCode.D, zones[2]);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ClickZone(KeyCode key, TargetClickZone zone)
    {
        if (Input.GetKeyDown(key))
        {
            TargetObject target;
            if (zone.Click(out target))
            {
                score += target.scoreValue;
                successSound.Play();
            }
            else
            {
                score -= 1;
                if (score < 0)
                    score = 0;
                failSound.Play();
            }
        }
    }

    private void FailedToClickInTime(object sender)
    {
        score -= 1;
        if (score < 0)
            score = 0;
        failSound.Play();
    }
}
