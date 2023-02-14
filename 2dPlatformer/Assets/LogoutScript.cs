using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoutScript : MonoBehaviour
{
    public void LogOut()
    {
        PlayerPrefs.SetString("playerLoginID", null);
        Application.Quit();
    }
}
