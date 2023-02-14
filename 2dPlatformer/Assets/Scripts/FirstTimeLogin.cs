using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class FirstTimeLogin : MonoBehaviour
{
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private TMPro.TMP_Text playerNameText;

    private string playerID
    {
        get
        {
            return PlayerPrefs.GetString("playerLoginID", null);
        }

        set
        {
            PlayerPrefs.SetString("playerLoginID", value);
        }
    }

    private void Awake()
    {
        Debug.Log(playerID);
        if (playerID == null || playerID == "")
        {
            loginWindow.SetActive(true);
        }
        else
        {
            Login();
        }
    }

    public void CheckName(TMPro.TMP_InputField input)
    {
        string name = Regex.Replace(input.text, @"[^0-9a-zA-Z]+", "");
        char[] chars = name.ToCharArray();
        name = "";

        foreach(char _char in chars)
        {
            if(name.Length != 16)
                name += _char;
        }

        input.text = name;
    }

    public void SetName(TMPro.TMP_InputField input)
    {
        CheckName(input);
        playerID = input.text;
        Login();
    }

    private void Login()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            CustomId = playerID,
            TitleId = "3D9AA"
        }, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        var request = new UpdateUserTitleDisplayNameRequest() { DisplayName = playerID };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, null, null);

        playerNameText.text = playerID;
        Debug.Log("Congratulations, you made a successful API call!");
        loginWindow.SetActive(false);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call.");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        loginWindow.SetActive(false);
    }
}
