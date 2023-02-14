using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpDisplay : MonoBehaviour
{
    [SerializeField] private Color colorActivated;
    [SerializeField] private Color colorDeactivated;
    [SerializeField] private List<GameObject> icons;
    protected Color ColorActivated => colorActivated;
    protected Color ColorDeactivated => colorDeactivated;
    protected List<GameObject> Icons => icons;

    private void Update()
    {
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerMovement = playerGO.GetComponent<PlayerMovement>();

        for (int i = 0; i <= icons.Count - 1; i++)
        {
            icons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        for (int i = 0; i <= playerMovement.MaxMidAirJumps - 1; i++)
        {
            icons[i].GetComponent<Image>().color = colorDeactivated;
        }

        for (int j = 0; j <= playerMovement.CurrentMidAirJumps - 1; j++)
        {
            icons[j].GetComponent<Image>().color = colorActivated;
        }
    }
}
