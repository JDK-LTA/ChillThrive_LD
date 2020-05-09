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
    public Image water0, water1, air0, air1, thermometer0, thermometer1;

    public float growth = 0f;
    public float growthFactor = 1f;
    public float growthFactorWhenIll = 0.5f;
    public float growthFactorWhenTwoIll = 0.25f;
    public int growthLevel = 0;
    public float[] growthCheckpoints;

    public Sprite[] stages;
    SpriteRenderer sr;
    public static bool end = false;
    float endingWater = 50f, endingAir = 50f, endingTemp = 15f;

    private void Start()
    {
        originalWDOT = waterDecreaseOverTime;
        originalADOT = airDecreaseOverTime;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = stages[growthLevel];
    }

    private void Update()
    {
        if (end)
        {
            waterLevel = endingWater;
            airLevel = endingAir;
            temperature = endingTemp;
        }
        else
        {
            waterLevel -= Time.deltaTime * waterDecreaseOverTime;

            //if (growthLevel >= 1)
            //{
            airLevel -= Time.deltaTime * airDecreaseOverTime;
            //}

            if (waterLevel <= 0)
            {
                Dead(PlantState.THIRSTY);
            }
            else if (waterLevel >= 100)
            {
                Dead(PlantState.DROWNING);
            }
            if (temperature <= -10)
            {
                Dead(PlantState.FROZEN);
            }
            else if (temperature >= 40)
            {
                Dead(PlantState.DRY);
            }
            if (airLevel <= 0)
            {
                Dead(PlantState.CHOKING);
            }
            else if (airLevel >= 100)
            {
                Dead(PlantState.ANXIOUS);
            }


            growth += growthFactor * Time.deltaTime;
            if (growthLevel < growthCheckpoints.Length)
            {
                if (growth > growthCheckpoints[growthLevel])
                {
                    growthLevel++;
                    if (growthLevel < growthCheckpoints.Length)
                    {
                        sr.sprite = stages[growthLevel];
                    }
                    else
                    {
                        Win();
                    }
                }
            }

            UpdateReceptionsAndWastes();
            UpdateBars();

        }
    }

    public void UpdateReceptionsAndWastes()
    {
        //SI TEMPERATURA == 30 && AGUA == 50 -> RECEPCION DE AIRE == 1 ____ AGUA >= 50 AIRREC--  ____ 
        airReception = 1 + ((temperature - temperatureThresholdEffectOnAirReception) / 50f);
        if (waterLevel > 50)
        {
            airReception -= waterLevel / 500;
        }

        waterReception = 1 + ((temperature - temperatureThresholdEffectOnWaterReception) / 50f);
        if (airLevel > 50)
        {
            waterReception -= airLevel / 500;
        }

        temperatureReception = 1 - (waterLevel / 200 + airLevel / 200);

        //SI TEMPERATURA == 35 -> DECREASERS == 150% ORIGINAL || SI TEMP == -5 -> DECREASERS == 50% ORIGINAL
        float t = Mathf.Clamp(temperature, -5, 35);
        t = (t + 5) / 40;
        airDecreaseOverTime = Mathf.Lerp(originalADOT * 0.5f, originalADOT * 1.5f, t);
        waterDecreaseOverTime = Mathf.Lerp(originalWDOT * 0.5f, originalWDOT * 1.5f, t);
        //airDecreaseOverTime = originalADOT + originalADOT * ((temperature - temperatureThresholdEffectOnAirReception) / 50f);
        //waterDecreaseOverTime = originalWDOT + originalWDOT * ((temperature - temperatureThresholdEffectOnWaterReception) / 50f);
    }

    public void StateChange(PlantState state, bool add)
    {
        if (add)
        {
            //interface
            switch (state)
            {
                case PlantState.FROZEN: thermometer0.gameObject.SetActive(true); break;
                case PlantState.DRY: thermometer1.gameObject.SetActive(true); break;
                case PlantState.THIRSTY: water0.gameObject.SetActive(true); break;
                case PlantState.DROWNING: water1.gameObject.SetActive(true); break;
                case PlantState.CHOKING: air0.gameObject.SetActive(true); break;
                case PlantState.ANXIOUS: air1.gameObject.SetActive(true); break;
            }

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
            //interface 
            switch (state)
            {
                case PlantState.FROZEN: thermometer0.gameObject.SetActive(false); break;
                case PlantState.DRY: thermometer1.gameObject.SetActive(false); break;
                case PlantState.THIRSTY: water0.gameObject.SetActive(false); break;
                case PlantState.DROWNING: water1.gameObject.SetActive(false); break;
                case PlantState.CHOKING: air0.gameObject.SetActive(false); break;
                case PlantState.ANXIOUS: air1.gameObject.SetActive(false); break;
            }
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
        if (waterBar != null && thermometer != null && airBar != null)
        {
            waterBar.fillAmount = waterLevel / 100f;
            thermometer.fillAmount = (temperature + 10f) / 50f; //De -10 a 40 grados centígrados
            airBar.fillAmount = airLevel / 100f;
        }
    }


    public MainMenuManager menuThing;
    public void Dead()
    {
        menuThing.gameObject.SetActive(true);
        menuThing.ChangeText(menuThing.transform.Find("Text").GetComponent<Text>(), "The plant died because all the parameters were below or over the thresholds :C");
        end = true;
    }
    public void Dead(PlantState state)
    {
        string aa = "";
        switch (state)
        {
            case PlantState.HEALTHY:
                break;
            case PlantState.DROWNING:
                aa = "DROWNED";
                break;
            case PlantState.THIRSTY:
                aa = "was THIRSTY";
                break;
            case PlantState.DRY:
                aa = "DRIED OUT";
                break;
            case PlantState.FROZEN:
                aa = "FROZE TO DEATH";
                break;
            case PlantState.ANXIOUS:
                aa = "HYPERVENTILATED";
                break;
            case PlantState.CHOKING:
                aa = "SUFFOCATED";
                break;
            default:
                break;
        }
        menuThing.gameObject.SetActive(true);
        menuThing.ChangeText(menuThing.transform.Find("Text").GetComponent<Text>(), "The plant died.\nIt " + aa + " :C");
        end = true;
    }

    public void Win()
    {
        menuThing.gameObject.SetActive(true);
        menuThing.ChangeText(menuThing.transform.Find("Text").GetComponent<Text>(), "The plant is alive and healthy :) You managed to help it thrive ♥");
        endingAir = airLevel;
        endingWater = waterLevel;
        endingTemp = temperature;
        end = true;
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
