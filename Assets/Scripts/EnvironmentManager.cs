using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    WaterManager wa = WaterManager.Instance;
    SunManager su = SunManager.Instance;
    WindManager wi = WindManager.Instance;

    void UpdateCovariances()
    {
        wi.airDecreaser += (SeedStateManager.Instance.stats.temperature - 15)/100;
    }
}
