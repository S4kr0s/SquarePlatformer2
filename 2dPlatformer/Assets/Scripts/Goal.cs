using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public static event Action OnGoalReached;

    private void Awake()
    {
        OnGoalReached += Win;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            OnGoalReached?.Invoke();
        }
    }

    private void Win()
    {
        Debug.Log("Won!");
        Time.timeScale = 0f;
    }
}
