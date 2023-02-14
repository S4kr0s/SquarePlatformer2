using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardGetter : MonoBehaviour
{
    [SerializeField] private LeaderboardElement[] leaderboardElements;

    private void OnEnable()
    {
        RequestLeaderboard();
    }

    private void DisplayLeaderboard(GetLeaderboardResult result)
    {
        List<PlayerLeaderboardEntry> leaderBoardEntries = result.Leaderboard;
        leaderBoardEntries.Reverse();

        for (int i = 0; i < leaderboardElements.Length; i++)
        {
            if (leaderBoardEntries.Count > i)
            {
                leaderboardElements[i].gameObject.SetActive(true);
                leaderboardElements[i].PlayerNameText.text = $"{i+1}. " + leaderBoardEntries[i].DisplayName;
                float score = leaderBoardEntries[i].StatValue / 1000f;
                leaderboardElements[i].PlayerScoreText.text = score.ToString("##0.000") + " seconds";
            }
            else
            {
                leaderboardElements[i].gameObject.SetActive(false);
            }
        }
    }

    private void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = LevelStatsDisplay.identifier,
            StartPosition = 0,
            MaxResultsCount = 100
        }, result => DisplayLeaderboard(result), FailureCallback);
    }


    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
