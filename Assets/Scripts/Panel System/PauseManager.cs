using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        isPaused = false;
    }

    private void Update()
    {
        if (GameManager.instance == null)
            return;

        if (GameManager.instance.IsGameEnded())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        isPaused = true;

        AudioManager.instance?.StopAllAudio();

        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (!isPaused)
            return;

        isPaused = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AudioManager.instance?.PlayIdle();
    }

    public void RestartGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        GameManager.instance.RestartGame();
    }

    public void MainMenu()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        AudioManager.instance?.StopAllAudio();

        GameManager.instance.LoadMainMenu();
    }
}