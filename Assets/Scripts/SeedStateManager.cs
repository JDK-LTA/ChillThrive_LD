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
    public GameObject seed, sun;
    [HideInInspector] public SeedStats stats;
    public Threshold tempTH, waterTH, airTH;
    #endregion

    private void Start()
    {
        stats = seed.GetComponent<SeedStats>();
    }

    float auxTimerAD;
    private void Update()
    {
        WaterManager.Instance.UpdateWaterStat(tempTH, waterTH, airTH);
        if (stats.growthLevel >= 1)
        {
            SunManager.Instance.UpdateTempStat(tempTH, waterTH, airTH);
        }
        if (stats.growthLevel >= 2)
        {
            WindManager.Instance.UpdateAirStat(tempTH, waterTH, airTH);
        }
    }
}