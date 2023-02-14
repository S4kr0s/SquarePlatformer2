using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardSubmitter : MonoBehaviour
{
    private void Awake()
    {
        Goal.OnGoalReached += SubmitScore;
    }

    private void OnDestroy()
    {
        Goal.OnGoalReached -= SubmitScore;
    }

    private void SubmitScore()
    {
        float score = float.Parse(LevelTimer.LevelTime.ToString("##0.000"));
        int value = (int)(score * 1000);

        LevelStats.SetLevelTime((Level)Enum.Parse(typeof(Level), LevelStatsDisplay.identifier), score);

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = LevelStatsDisplay.identifier,
                Value = ((int)(score * 1000)),
            }
        }
        }, result => OnStatisticsUpdated(result), FailureCallback);
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
