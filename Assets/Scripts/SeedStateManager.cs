using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantState
{
    HEALTHY,
    DROWNING,
    DRY,
    CHOKING,
    FROZEN
}
public class SeedStateManager : MonoBehaviour
{
    public static SeedStateManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    PlantState plantState = PlantState.HEALTHY;
    PlantState plantSecondaryState = PlantState.HEALTHY;

    #region Variables
    public int level = 1;
    public GameObject seed, sun;
    SeedStats stats;
    public DragSun dragSun;
    public BezierPath sunPath;
    public float sunXHit = 5, waterXHit = 5, airXHit = 5;

    public float timerXSunHit = 1, timerXWaterHit = 1, timerXAirHit = 1;
    public float sunFactor, waterFactor, airFactor, cloudFactor, rainFactor;

    public float rainThreshold = 0.7f;
    private bool isRaining, isStorming, isDay;
    #endregion

    private void Start()
    {
        stats = seed.GetComponent<SeedStats>();
    }

    float auxTimer0, auxTimer1, auxTimer2;
    private void Update()
    {
        #region Timers
        auxTimer0 += Time.deltaTime;
        if (auxTimer0 >= timerXSunHit)
        {
            UpdateSunStat();
            auxTimer0 = 0;
        }

        auxTimer1 += Time.deltaTime;
        if (auxTimer1>=timerXWaterHit)
        {
            UpdateWaterStat();
            auxTimer1 = 0;
        }

        auxTimer2 += Time.deltaTime;
        if (auxTimer2>=timerXAirHit)
        {
            UpdateAirStat();
            auxTimer2 = 0;
        }
        #endregion


    }

    #region Factor updaters
    public void UpdateSunFactor()
    {
        //Debug.Log(dragSun.position / (sunPath.numOfPoints / 2));
        //Debug.Log(dragSun.position);
        if ((float)dragSun.position / (float)sunPath.numOfPoints < 0.05f || (float)dragSun.position / (float)sunPath.numOfPoints > 0.95f)
        {
            isDay = false;
            sunFactor = 0f;
        }
        else if ((float)dragSun.position/ (float)sunPath.numOfPoints < 0.5f)
        {
            sunFactor = Mathf.Lerp(0.1f, 1, (float)dragSun.position / ((float)sunPath.numOfPoints / 2));
        }
        else
        {
            sunFactor = Mathf.Lerp(1, 0.1f, (((float)dragSun.position / ((float)sunPath.numOfPoints / 2)) - 1));
        }

        sunFactor *= 1 - cloudFactor;
    }

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
    public void UpdateAirFactor(float value)
    {
        airFactor = value;
    }
    #endregion

    #region Stat updaters
    private void UpdateSunStat()
    {
        stats.temperature += sunXHit * sunFactor;
    }
    private void UpdateWaterStat()
    {
        stats.waterLevel += waterXHit * waterFactor;
    }
    private void UpdateAirStat()
    {
        stats.airLevel += airXHit * airFactor;
    }
    #endregion
}
