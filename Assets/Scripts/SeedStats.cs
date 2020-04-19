using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedStats : MonoBehaviour
{
    PlantState mainState = PlantState.HEALTHY;
    PlantState secondaryState = PlantState.HEALTHY;

    int nOfConditions = 0;

    public float waterLevel = 100f, temperature = 15f, airLevel = 100f;
    public float waterDecreaseOverTime = 3, airDecreaseOverTime = 3;
    public Image waterBar, thermometer, airBar;

    public float growth = 0f;
    public int growthLevel = 0;
    public float[] growthCheckpoints;

    private void Update()
    {
        waterLevel -= Time.deltaTime * waterDecreaseOverTime;

        if (growthLevel >= 1)
        {
            airLevel -= Time.deltaTime * airDecreaseOverTime;
        }

        if (waterLevel <= 0 || waterLevel >= 100 || temperature <= -10 || temperature >= 40 || airLevel <= 0 || airLevel >= 100)
        {
            Dead();
        }

        //if(growth > growthCheckpoints[growthLevel])
        //{
        //    growthLevel++;
        //    //NUEVO NIVEL
        //}

        UpdateBars();
    }

    public void StateChange(PlantState state)
    {
        if (mainState == PlantState.HEALTHY)
        {
            mainState = state;
        }
        else if (secondaryState == PlantState.HEALTHY)
        {
            secondaryState = state;
        }
        else
        {
            Dead();
        }
    }
    public void StateChange()
    {
        if (secondaryState != PlantState.HEALTHY)
        {
            mainState = secondaryState;
        }
        else
        {
            mainState = PlantState.HEALTHY;
        }
        secondaryState = PlantState.HEALTHY;
    }

    public void UpdateBars()
    {
        waterBar.fillAmount = waterLevel / 100f;
        thermometer.fillAmount = temperature + 20f / 50f; //De -10 a 40 grados centígrados
        airBar.fillAmount = airLevel / 100f;
    }

    public void Dead()
    {
        Debug.Log("Plant is dead");
    }
}

public enum PlantState
{
    HEALTHY,
    DROWNING,
    THIRSTY,
    DRY,
    FROZEN,
    ANXIOUS,
    CHOKING
}
