using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public Color duringDay;
    public Color duringNight;

    public void Change(bool isDay)
    {
        if (isDay) GetComponent<Image>().color = duringDay;
        else GetComponent<Image>().color = duringNight;
    }
}
