using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowScore : MonoBehaviour
{
    private GamelogicManager manager;
    private Text scoretext;

    private void Awake()
    {
        manager = FindObjectOfType<GamelogicManager>();
        scoretext = GetComponent<Text>();
    }

    private void Update()
    {
        scoretext.text = manager.score.ToString();
    }
}
