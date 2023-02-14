using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    private string _name;
    private bool _isBeaten;
    private float _bestTimeBeaten;
    private string _versionBeaten;

    public string Name => _name;
    public bool IsBeaten => _isBeaten;
    public float BestTime => _bestTimeBeaten;
    public string Version => _versionBeaten;

    private LevelStats(Level level)
    {
        this._name = PlayerPrefs.GetString($"{level}name", "Level or Name not Defined.");
        this._isBeaten = PlayerPrefs.GetInt($"{level}beaten", 0) == 1 ? true : false;
        this._bestTimeBeaten = PlayerPrefs.GetFloat($"{level}time", 0);
        this._versionBeaten = PlayerPrefs.GetString($"{level}version", "Level or Version not Defined.");
    }

    public static void SetLevelTime(Level level, float bestTime)
    {
        if(PlayerPrefs.GetFloat($"{level}time") == 0 || bestTime < PlayerPrefs.GetFloat($"{level}time"))
        {
            PlayerPrefs.SetFloat($"{level}time", bestTime);
            PlayerPrefs.SetInt($"{level}beaten", 1);
        }
    }

    public static LevelStats GetLevelStats(Level level)
    {
        LevelStats levelStats = new LevelStats(level);
        return levelStats;
    }

    public static void CreateAllLevelStats()
    {
        foreach (Level level in (Level[]) Enum.GetValues(typeof(Level)))
        {
            GenerateLevelStats(level);
        }
    }

    public static void CreateLevelStats(Level level)
    {
        GenerateLevelStats(level);
    }

    private static void GenerateLevelStats(Level level)
    {
        string name = level.ToString();
        name = name.Replace("world", "World ");
        name = name.Replace("level", " Level ");

        switch (level)
        {
            case Level.world1level01:
                name = "Start of the Journey";
                break;
            case Level.world1level02:
                name = "Cave Entrance";
                break;
            case Level.world1level03:
                name = "Pathway above the Spikes";
                break;
            case Level.world1level04:
                name = "Seemingly flying";
                break;
            case Level.world1level05:
                name = "Faster than Flash";
                break;
            case Level.world1level06:
                name = "Tight Fall";
                break;
            case Level.world1level07:
                name = "Maze through the Spikes";
                break;
            case Level.world1level08:
                name = "All around it";
                break;
            case Level.world1level09:
                name = "Show off your mechanics";
                break;
            case Level.world1level10:
                name = "The final level.. for now..";
                break;
            case Level.world2level01:
                //name = "";
                break;
            case Level.world2level02:
                //name = "";
                break;
            case Level.world2level03:
                //name = "";
                break;
            case Level.world2level04:
                //name = "";
                break;
            case Level.world2level05:
                //name = "";
                break;
            case Level.world2level06:
                //name = "";
                break;
            case Level.world2level07:
                //name = "";
                break;
            case Level.world2level08:
                //name = "";
                break;
            case Level.world2level09:
                //name = "";
                break;
            case Level.world2level10:
                //name = "";
                break;
            case Level.world3level01:
                //name = "";
                break;
            case Level.world3level02:
                //name = "";
                break;
            case Level.world3level03:
                //name = "";
                break;
            case Level.world3level04:
                //name = "";
                break;
            case Level.world3level05:
                //name = "";
                break;
            case Level.world3level06:
                //name = "";
                break;
            case Level.world3level07:
                //name = "";
                break;
            case Level.world3level08:
                //name = "";
                break;
            case Level.world3level09:
                //name = "";
                break;
            case Level.world3level10:
                //name = "";
                break;
            default:
                break;
        }

        PlayerPrefs.SetString($"{level}name", name);

        if (PlayerPrefs.GetString($"{level}name", "Level or Name not Defined.") == "Level or Name not Defined.")
        {
            PlayerPrefs.SetString($"{level}name", name);
            PlayerPrefs.SetInt($"{level}beaten", 0);
            PlayerPrefs.SetFloat($"{level}time", 0);
            PlayerPrefs.SetString($"{level}version", Game.Version);
        }

        if (PlayerPrefs.GetString($"{level}version", "Level or Version not Defined.") == "Level or Version not Defined." ||
            PlayerPrefs.GetString($"{level}version") != Game.Version)
        {
            PlayerPrefs.SetFloat($"{level}time", 0);
            PlayerPrefs.SetString($"{level}version", Game.Version);
        }
    }
}

public enum Level
{
    world1level01,
    world1level02,
    world1level03,
    world1level04,
    world1level05,
    world1level06,
    world1level07,
    world1level08,
    world1level09,
    world1level10,
    world2level01,
    world2level02,
    world2level03,
    world2level04,
    world2level05,
    world2level06,
    world2level07,
    world2level08,
    world2level09,
    world2level10,
    world3level01,
    world3level02,
    world3level03,
    world3level04,
    world3level05,
    world3level06,
    world3level07,
    world3level08,
    world3level09,
    world3level10,
}
