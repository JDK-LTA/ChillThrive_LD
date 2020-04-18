using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedStats : MonoBehaviour
{
    public float waterLevel = 100f, temperature = 20f, airLevel = 100f;

    public Image waterBar, sunBar, airBar;

    private void Update()
    {
        UpdateBars();
    }
    public void UpdateBars()
    {
        waterBar.fillAmount = waterLevel / 100f;
        sunBar.fillAmount = temperature + 20f / 70f; //De -20 a 50 grados centígrados
        airBar.fillAmount = airLevel / 100f;
    }
}
