using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedStats : MonoBehaviour
{
    public PlantState mainState = PlantState.HEALTHY;
    public PlantState secondaryState = PlantState.HEALTHY;

    int nOfConditions = 0;

    public float waterLevel = 100f, temperature = 15f, airLevel = 100f;
    public float waterReception = 1f;
    public float airReception = 1f;
    public float temperatureReception = 1f;
    public float temperatureThresholdEffectOnAirReception = 15f;
    public float temperatureThresholdEffectOnWaterReception = 15f;
    public float waterDecreaseOverTime = 0.1f, airDecreaseOverTime = 0.1f;
    float originalWDOT, originalADOT;
    public Image waterBar, thermometer, airBar;

    public float growth = 0f;
    public float growthFactor = 1f;
    public float growthFactorWhenIll = 0.5f;
    public float growthFactorWhenTwoIll = 0.25f;
    public int growthLevel = 0;
    public float[] growthCheckpoints;

    private void Start()
    {
        originalWDOT = waterDecreaseOverTime;
        originalADOT = airDecreaseOverTime;
    }
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


        growth += growthFactor * Time.deltaTime;
        if (growthLevel < growthCheckpoints.Length)
        {
            if (growth > growthCheckpoints[growthLevel])
            {
                growthLevel++;
                // NUEVO NIVEL
            }
        }

        UpdateReceptionsAndWastes();
        UpdateBars();
    }

    public void UpdateReceptionsAndWastes()
    {
        //SI TEMPERATURA == 30 && AGUA == 50 -> RECEPCION DE AIRE == 1 ____ AGUA >= 50 AIRREC--  ____ 
        airReception = 1 + ((temperature - temperatureThresholdEffectOnAirReception) / 50f);
        if (waterLevel > 50)
        {
            airReception -= waterLevel / 500;
        }

        waterReception =  1 + ((temperature - temperatureThresholdEffectOnWaterReception) / 50f);
        if (airLevel > 50)
        {
            waterReception -= airLevel / 500;
        }

        temperatureReception = 1 - (waterLevel / 400 + airLevel / 400);

        airDecreaseOverTime = originalADOT * ((temperature - temperatureThresholdEffectOnAirReception) / 50f);
        waterDecreaseOverTime = originalWDOT * ((temperature - temperatureThresholdEffectOnWaterReception) / 50f);
    }

    public void StateChange(PlantState state, bool add)
    {
        if (add)
        {
            if (mainState == PlantState.HEALTHY)
            {
                growthFactor = growthFactorWhenIll;
                mainState = state;
            }
            else if (secondaryState == PlantState.HEALTHY)
            {
                growthFactor = growthFactorWhenTwoIll;
                secondaryState = state;
            }
            else
            {
                Dead();
            }
        }
        else
        {
            if (mainState == state)
            {
                mainState = secondaryState;
                secondaryState = PlantState.HEALTHY;
            }
            else if (secondaryState == state)
            {
                secondaryState = PlantState.HEALTHY;
            }
        }

    }

    public void UpdateBars()
    {
        waterBar.fillAmount = waterLevel / 100f;
        thermometer.fillAmount = (temperature + 10f) / 50f; //De -10 a 40 grados centígrados
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
