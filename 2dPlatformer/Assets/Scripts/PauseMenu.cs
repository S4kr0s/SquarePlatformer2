using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMPro.TMP_Text titleText;
    [SerializeField] private TMPro.TMP_Text levelTimeText;
    [SerializeField] private GameObject nextLevelButton;
    private bool won = false;

    private void Awake()
    {
        Goal.OnGoalReached += HandleGoalReached;
    }

    private void OnDestroy()
    {
        Goal.OnGoalReached -= HandleGoalReached;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeSelf && !won)
            OpenPauseMenu();
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf && !won)
            ClosePauseMenu();
    }

    private void HandleGoalReached()
    {
        won = true;
        OpenPauseMenu();
        ConfigureWinMenu();
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        ConfigurePauseMenu();
    }

    private void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    private void ConfigurePauseMenu()
    {
        nextLevelButton.SetActive(false);
        titleText.text = "Level Paused.";
        levelTimeText.gameObject.SetActive(false);
    }

    private void ConfigureWinMenu()
    {
        nextLevelButton.SetActive(true);
        titleText.text = "Level Won!";
        levelTimeText.gameObject.SetActive(true);
        levelTimeText.text = $"{LevelTimer.LevelTime:##0.000} seconds";
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log(NameOfSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1));
            LevelStatsDisplay.identifier = NameOfSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public static string NameOfSceneByBuildIndex(int buildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
}
