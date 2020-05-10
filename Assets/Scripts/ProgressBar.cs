using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();        
    }
    private void Update()
    {
        image.fillAmount = 
            SeedStateManager.Instance.stats.growth / 
            SeedStateManager.Instance.stats.growthCheckpoints[SeedStateManager.Instance.stats.growthCheckpoints.Length - 1];
    }
}
