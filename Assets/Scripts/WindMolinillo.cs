using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMolinillo : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, -1), WindManager.Instance.airFactor * 180 * Time.deltaTime);
    }
}
