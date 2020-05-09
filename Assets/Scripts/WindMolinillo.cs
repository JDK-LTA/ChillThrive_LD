using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMolinillo : MonoBehaviour
{
    public int speed;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, -1), WindManager.Instance.airFactor * speed * Time.deltaTime);
    }
}
