using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public float airFactor;
    public float airXHit = 0.1f;
    public float airDecreaser = 0.01f;
    public float airDecreaserTimer = 0.25f;

    float auxTimerAD = 0;

    void Update()
    {
        if (airFactor > 0)
        {
            auxTimerAD += Time.deltaTime;
            if (auxTimerAD > airDecreaserTimer)
            {
                airFactor -= airDecreaser;
                auxTimerAD = 0;
            }
        }
    }

    public void UpdateAirFactor(float value)
    {
        airFactor = value;
    }
    bool airOff = false;
    public void UpdateAirStat(Threshold tempTH, Threshold waterTH, Threshold airTH)
    {
        SeedStateManager ssm = SeedStateManager.Instance;
        ssm.stats.airLevel += airXHit * airFactor * Time.deltaTime;

        float actualUpTh = airTH.up, actualDownTh = airTH.down;
        if (ssm.stats.temperature > tempTH.up) // CAMBIOS SEGUN OTROS PARAMETROS COMO VIENTO O AGUA
        {
            actualUpTh -= tempTH.outsideThChanger;
        }
        else if (ssm.stats.temperature < tempTH.down)
        {
            actualDownTh += tempTH.outsideThChanger;
            actualUpTh += tempTH.outsideThChanger;
        }


        if (!airOff)
        {
            if (ssm.stats.airLevel >= actualUpTh)
            {
                ssm.stats.StateChange(PlantState.ANXIOUS);
                airOff = true;
            }
            else if (ssm.stats.airLevel <= actualDownTh)
            {
                ssm.stats.StateChange(PlantState.CHOKING);
                airOff = true;
            }
        }
        else if (ssm.stats.airLevel < actualUpTh || ssm.stats.airLevel > actualDownTh)
        {
            airOff = false;
            ssm.stats.StateChange();
        }
    }
}
