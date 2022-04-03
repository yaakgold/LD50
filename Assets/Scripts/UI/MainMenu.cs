using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScore;
    public GameObject mainMenu;
    public GameObject tutorialScreen;

    private void Start()
    {
        highScore.text = $"High Score: {PlayerPrefs.GetInt(GameManager.ScoreString, 0)}";
    }

    public void StartGame()
    {
        AudioManager.instance?.Play("BtnClick");
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        AudioManager.instance?.Play("BtnClick");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Tutorial()
    {
        AudioManager.instance?.Play("BtnClick");

        mainMenu.SetActive(false);
        tutorialScreen.SetActive(true);
    }

    public void ExitTutorial()
    {
        AudioManager.instance?.Play("BtnClick");

        mainMenu.SetActive(true );
        tutorialScreen.SetActive(false);
    }
}
