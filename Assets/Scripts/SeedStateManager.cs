using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Threshold
{
    public float up, down;
    public float outsideThChanger = 5;
}
public class SeedStateManager : MonoBehaviour
{
    public static SeedStateManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    #region Variables
    public GameObject seed;
    [HideInInspector] public SeedStats stats;
    public Threshold tempTH, waterTH, airTH;

    public bool isDay = true;
    #endregion

    private void Start()
    {
        stats = seed.GetComponent<SeedStats>();
    }

    float auxTimerAD;
    private void Update()
    {
        if (!SeedStats.end)
        {
            WaterManager.Instance.UpdateWaterStat(tempTH, waterTH, airTH);
            SunManager.Instance.UpdateTempStat(tempTH, waterTH, airTH);
            WindManager.Instance.UpdateAirStat(tempTH, waterTH, airTH);
        }        
    }
}