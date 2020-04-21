using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Juego");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void ChangeText(Text text, string st)
    {
        text.text = st;
    }
}
