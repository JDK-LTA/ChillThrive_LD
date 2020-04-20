using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public static WaterManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<ShakeCloud> cloudsRaining = new List<ShakeCloud>();
    public List<ShakeCloud> cloudsAlive = new List<ShakeCloud>();

    public float rainFactor;
    public float cloudFactor;
    public float rainThreshold = 0.7f;
    private bool isRaining, isStorming;

    public float waterFactor;
    public float waterXHit = 0.3f;

    bool waterOff = false;

    public void UpdateWaterFactor(float value)
    {
        value += value * rainFactor;
        waterFactor = value;
    }

    //TODO: ARREGLAR EL CLOUD FACTOR
    public void UpdateCloudFactor(float value)
    {
        cloudFactor = value;
        //if (cloudFactor > rainThreshold)
        //{
        //    isRaining = true;
        //    rainFactor = Mathf.Lerp(0, 1, (cloudFactor - rainThreshold) / (1 - rainThreshold));
        //}
    }

    private void Update()
    {
        rainFactor = 0;
        for (int i = 0; i < cloudsRaining.Count; i++)
        {
            rainFactor += cloudsRaining[i].rainPower;            
        }
        rainFactor = Mathf.Clamp(rainFactor, 0f, 1f);
    }

    public void UpdateWaterStat(Threshold tempTH, Threshold waterTH, Threshold airTH)
    {
        SeedStateManager ssm = SeedStateManager.Instance;
        ssm.stats.waterLevel += waterXHit * rainFactor * ssm.stats.waterReception * Time.deltaTime;

        float actualUpTh = waterTH.up, actualDownTh = waterTH.down;
        if (ssm.stats.temperature > tempTH.up) // CAMBIOS SEGUN OTROS PARAMETROS COMO VIENTO O AGUA
        {
            actualDownTh += tempTH.outsideThChanger;
            actualUpTh += tempTH.outsideThChanger;
        }
        else if (ssm.stats.temperature < tempTH.down)
        {
            actualDownTh -= tempTH.outsideThChanger;
            actualUpTh -= tempTH.outsideThChanger;
        }

        //if (stats.airLevel > airTH.up)
        //{
        //    actualDownTh += airTH.outsideThChanger;
        //}
        //else if (stats.airLevel < airTH.down)
        //{
        //    actualUpTh -= airTH.outsideThChanger;
        //}

        if (!waterOff)
        {
            if (ssm.stats.waterLevel >= actualUpTh)
            {
                ssm.stats.StateChange(PlantState.DROWNING, true);
                waterOff = true;
            }
            else if (ssm.stats.waterLevel <= actualDownTh)
            {
                ssm.stats.StateChange(PlantState.THIRSTY, true);
                waterOff = true;
            }
        }
        else if (ssm.stats.waterLevel < actualUpTh && ssm.stats.waterLevel > actualDownTh)
        {
            waterOff = false;
            ssm.stats.StateChange(PlantState.DROWNING, false);
            ssm.stats.StateChange(PlantState.THIRSTY, false);
        }
    }
}
