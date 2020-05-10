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

        if (transform.childCount>0)
        {

            if (isDay) transform.GetChild(0).GetComponent<Image>().color = duringDay;
            else transform.GetChild(0).GetComponent<Image>().color = duringNight;
        }
    }
}
