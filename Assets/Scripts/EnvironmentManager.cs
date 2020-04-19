using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    WaterManager wa = WaterManager.Instance;
    SunManager su = SunManager.Instance;
    WindManager wi = WindManager.Instance;

    public float waterVariance = 0.05f;
    public float temperaturVariance = 0.05f;
    public float airVariance = 0.05f;

    void UpdateCovariances()
    {
    }
}
