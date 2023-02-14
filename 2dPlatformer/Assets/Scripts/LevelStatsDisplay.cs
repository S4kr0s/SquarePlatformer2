using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelStatsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject actualContent;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textIsBeaten;
    [SerializeField] private TMP_Text textTime;
    [SerializeField] private TMP_Text textVersion;


    // Identifier of the level the stats will be displayed in.
    public static string identifier;

    public void ChangeSelectedLevel(string level)
    {
        identifier = level;

        try
        {
            LevelStats levelStats = LevelStats.GetLevelStats((Level)Enum.Parse(typeof(Level), identifier));

            textName.text = $"LEVEL NAME: {levelStats.Name}";
            textIsBeaten.gameObject.SetActive(!levelStats.IsBeaten);
            textTime.gameObject.SetActive(levelStats.IsBeaten);
            textVersion.gameObject.SetActive(levelStats.IsBeaten);
            textTime.text = $"BEST TIME: {levelStats.BestTime} SECONDS";
            textVersion.text = $"GAME VERSION: {levelStats.Version}";

            actualContent.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void LoadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(identifier);
    }

    public void BackToLevelSelect()
    {
        actualContent.SetActive(false);
    }
}
