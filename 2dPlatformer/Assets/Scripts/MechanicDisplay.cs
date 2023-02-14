using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanicDisplay : MonoBehaviour
{
    [SerializeField] private Color colorActivated;
    [SerializeField] private Color colorDeactivated;
    [SerializeField] private List<GameObject> icons;
    protected Color ColorActivated => colorActivated;
    protected Color ColorDeactivated => colorDeactivated;
    protected List<GameObject> Icons => icons;
}
