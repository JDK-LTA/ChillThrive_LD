using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialPanel, Button;
    bool active;

    private void Start()
    {
        active = false;
        TutorialPanel.SetActive(false);
        Invoke("StartTutorial", 5.0f);

    }

    private void Update()
    {
        if (active && Input.GetMouseButtonDown(0))
        {
            EndTutorial();
        }
    }

    public void ClickButton()
    {
        StartTutorial();
    }

    void StartTutorial()
    {
        active = true;
        TutorialPanel.SetActive(true);
        Button.SetActive(false);
        SeedStateManager.Instance.stats.StopMeters();
    }

    void EndTutorial()
    {
        active = false;
        TutorialPanel.SetActive(false);
        Button.SetActive(true);
        SeedStateManager.Instance.stats.RestartMeters();
    }

}
