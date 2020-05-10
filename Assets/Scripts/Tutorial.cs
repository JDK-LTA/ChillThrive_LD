using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        SetThisActive();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void SetThisActive()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.25f;
    }
}
