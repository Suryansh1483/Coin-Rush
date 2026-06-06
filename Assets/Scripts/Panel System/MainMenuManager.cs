using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject backgroundPanel;
    public GameObject howToPlayPanel;
    public GameObject bestScorePanel;

    [Header("High Score UI")]
    public TMP_Text scoresText;

    private void Start()
    {
        ShowMainMenu();

        AudioManager.instance?.PlayMenuMusic();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        AudioManager.instance?.StopMenuMusic();

        SceneManager.LoadScene("GAME Scene");
    }

    public void OpenHowToPlay()
    {
        AudioManager.instance?.PlayButtonClick();

        backgroundPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
        bestScorePanel.SetActive(false);
    }

    public void OpenHighScores()
    {
        AudioManager.instance?.PlayButtonClick();

        backgroundPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        bestScorePanel.SetActive(true);

        LoadScores();
    }

    public void BackToMenu()
    {
        AudioManager.instance?.PlayButtonClick();

        AudioManager.instance?.StopGameplayAudio();

        ShowMainMenu();

        AudioManager.instance?.PlayMenuMusic();
    }

    private void ShowMainMenu()
    {
        backgroundPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        bestScorePanel.SetActive(false);
    }

    private void LoadScores()
    {
        List<float> scores = HighScoreManager.GetScores();

        if (scores.Count == 0)
        {
            scoresText.text = "No Scores Yet";
            return;
        }

        string display = "";

        for (int i = 0; i < scores.Count; i++)
        {
            display +=
                (i + 1) +
                ". " +
                scores[i].ToString("F2") +
                " sec\n";
        }

        scoresText.text = display;
    }

    public void ClearScores()
    {
        AudioManager.instance?.PlayButtonClick();

        HighScoreManager.ClearScores();

        scoresText.text = "No Scores Yet";
    }

    public void QuitGame()
    {
        AudioManager.instance?.PlayButtonClick();

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}