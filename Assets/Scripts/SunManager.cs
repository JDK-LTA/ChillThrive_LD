using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    public static SunManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public DragSun dragSun;
    public BezierPath sunPath;
    public float tempFactor;
    public float tempXHit = 0.1f;

    bool isDay;
    public void UpdateTempFactor()
    {
        if ((float)dragSun.position / (float)sunPath.numOfPoints < 0.05f || (float)dragSun.position / (float)sunPath.numOfPoints > 0.95f)
        {
            isDay = false;
            tempFactor = 0f;
        }
        else if ((float)dragSun.position / (float)sunPath.numOfPoints < 0.5f)
        {
            tempFactor = Mathf.Lerp(0.1f, 1, (float)dragSun.position / ((float)sunPath.numOfPoints / 2));
        }
        else
        {
            tempFactor = Mathf.Lerp(1, 0.1f, (((float)dragSun.position / ((float)sunPath.numOfPoints / 2)) - 1));
        }

        tempFactor *= 1 - WaterManager.Instance.cloudFactor; // OJO CUIDAO QUE EL CLOUDFACTOR ESTA MAL POR AHORA
    }

    bool temperatureOff = false;
    public void UpdateTempStat(Threshold tempTH, Threshold waterTH, Threshold airTH)
    {
        SeedStateManager ssm = SeedStateManager.Instance;
        ssm.stats.temperature += tempXHit * tempFactor * Time.deltaTime;

        float actualUpTh = tempTH.up, actualDownTh = tempTH.down;
        if (ssm.stats.airLevel > airTH.up) // CAMBIOS SEGUN OTROS PARAMETROS COMO VIENTO O AGUA
        {
            actualDownTh += airTH.outsideThChanger;
        }
        else if (ssm.stats.airLevel < airTH.down)
        {
            actualUpTh -= airTH.outsideThChanger;
        }

        if (!temperatureOff)
        {
            if (ssm.stats.temperature >= actualUpTh)
            {
                ssm.stats.StateChange(PlantState.DRY);
                temperatureOff = true;
            }
            else if (ssm.stats.temperature <= actualDownTh)
            {
                ssm.stats.StateChange(PlantState.FROZEN);
                temperatureOff = true;
            }
        }
        else if (ssm.stats.temperature < actualUpTh || ssm.stats.temperature > actualDownTh)
        {
            temperatureOff = false;
            ssm.stats.StateChange();
        }
    }
}
