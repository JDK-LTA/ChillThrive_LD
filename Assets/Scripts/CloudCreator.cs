using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreator : MonoBehaviour
{
    public GameObject cloud;
    public Transform[] spawnPoints;
    public Transform cloudParent;
    public float timerOnWindZero = 5f;
    float originalTOWZ;
    public float minSpeed = 0.5f, maxSpeed = 3.5f;
    public float minRain = 0.1f, maxRain = 0.5f;

    float timer = 0;

    private void Start()
    {
        originalTOWZ = timerOnWindZero;
    }
    private void Update()
    {
        UpdateTimer();

        timer += Time.deltaTime;
        if (timer >= timerOnWindZero)
        {
            int i = Random.Range(0, spawnPoints.Length);
            GameObject go = Instantiate(cloud, spawnPoints[i].position, spawnPoints[i].rotation, cloudParent);

            ShakeCloud sc = go.GetComponent<ShakeCloud>();
            WaterManager.Instance.cloudsAlive.Add(sc);

            sc.speed = Random.Range(minSpeed, maxSpeed);
            sc.rainPower = (float)((int)Random.Range(minRain * 10, maxRain * 10)) / 10;
            if (spawnPoints[i].transform.position.x > 0)
            {
                sc.speed *= -1;
            }
            
            //WaterManager.Instance.AddCloudToList(sc, sc.rainPower);

            timer = 0;
        }
    }

    void UpdateTimer()
    {
        timerOnWindZero = originalTOWZ * (1 - (WindManager.Instance.airFactor / 2));
    }
}
