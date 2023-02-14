using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private static float levelTime;
    public static float LevelTime => levelTime;
    private bool countTime = true;

    private void Awake()
    {
        levelTime = 0;
        Goal.OnGoalReached += DisableTimer;
    }

    private void Update()
    {
        if (countTime && PlayerMovement.HasMoved)
            levelTime += Time.deltaTime;

        text.text = levelTime.ToString("000.000") + "\nseconds";
    }

    private void DisableTimer()
    {
        countTime = false;
    }
}
