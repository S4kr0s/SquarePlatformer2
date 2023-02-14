using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static string _version = "0.1a.0420";
    public static string Version => _version;

    private void Awake()
    {
        LevelStats.CreateAllLevelStats();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
