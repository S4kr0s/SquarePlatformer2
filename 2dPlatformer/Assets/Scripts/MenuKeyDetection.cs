using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKeyDetection : MonoBehaviour
{
    [SerializeField] private GameObject[] menues;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject menu in menues)
            {
                menu.SetActive(false);
            }
            menues[0].SetActive(true);
        }
    }
}
