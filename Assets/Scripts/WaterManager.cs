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

    public class RainingCloud
    {
        public RainingCloud(ShakeCloud sc, float value)
        {
            cloud = sc;
            rainingValue = value;
        }
        public ShakeCloud cloud;
        public float rainingValue;
    }

    public List<RainingCloud> rainingClouds = new List<RainingCloud>();

    public float rainFactor;
    public float cloudFactor;
    public float rainThreshold = 0.7f;
    private bool isRaining, isStorming;

    public float waterFactor;
    public float waterXHit = 0.1f;

    public void UpdateWaterFactor(float value)
    {
        value += value * rainFactor;
        waterFactor = value;
    }
    public void UpdateCloudFactor(float value)
    {
        cloudFactor = value;
        if (cloudFactor > rainThreshold)
        {
            isRaining = true;
            rainFactor = Mathf.Lerp(0, 1, (cloudFactor - rainThreshold) / (1 - rainThreshold));
        }
    }

    public void AddCloudToList(ShakeCloud cloud, float rainValue)
    {
        rainingClouds.Add(new RainingCloud(cloud, rainValue));
        cloudTimers.Add(0f);
    }
    public void DeleteCloudFromList(int i)
    {
        rainingClouds.Remove(rainingClouds[i]);
        cloudTimers.Remove(cloudTimers[i]);
    }

    public float timeToDecreaseRain = 1f;
    List<float> cloudTimers = new List<float>();
    private void Update()
    {
        rainFactor = 0;
        for (int i = 0; i < rainingClouds.Count; i++)
        {
            cloudTimers[i] += Time.deltaTime;
            if (cloudTimers[i] >= timeToDecreaseRain)
            {
                rainingClouds[i].rainingValue -= 0.1f;
                if (rainingClouds[i].rainingValue <= 0)
                {
                    DeleteCloudFromList(i);

                }

                cloudTimers[i] = 0;
            }

            rainFactor += rainingClouds[i].rainingValue;
        }
    }



    bool waterOff = false;
    public void UpdateWaterStat(Threshold tempTH, Threshold waterTH, Threshold airTH)
    {
        SeedStateManager ssm = SeedStateManager.Instance;
        ssm.stats.waterLevel += waterXHit * waterFactor * Time.deltaTime;

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
                ssm.stats.StateChange(PlantState.DROWNING);
                waterOff = true;
            }
            else if (ssm.stats.waterLevel <= actualDownTh)
            {
                ssm.stats.StateChange(PlantState.THIRSTY);
                waterOff = true;
            }
        }
        else if (ssm.stats.waterLevel < actualUpTh || ssm.stats.waterLevel > actualDownTh)
        {
            waterOff = false;
            ssm.stats.StateChange();
        }
    }
}
