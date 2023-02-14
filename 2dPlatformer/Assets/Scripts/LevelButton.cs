using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string identifier;
    [SerializeField] private Scene scene;
    [SerializeField] private GameObject normalStar;
    [SerializeField] private GameObject normalStarFilled;
    [SerializeField] private GameObject highlightStar;
    [SerializeField] private GameObject highlightStarFilled;

    private void Awake()
    {
        if (LevelStats.GetLevelStats((Level)Enum.Parse(typeof(Level), identifier)).IsBeaten)
        {
            normalStar.SetActive(false);
            highlightStar.SetActive(false);
            normalStarFilled.SetActive(true);
            highlightStarFilled.SetActive(true);
        }
        else
        {
            normalStar.SetActive(true);
            highlightStar.SetActive(true);
            normalStarFilled.SetActive(false);
            highlightStarFilled.SetActive(false);
        }
    }

    public void LoadLevelStats(string level)
    {
        GameObject.FindGameObjectWithTag("LevelStatsDisplay").GetComponent<LevelStatsDisplay>().ChangeSelectedLevel(level);
    }
}
