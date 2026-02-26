using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject MainPanel;
    public GameObject CreditsPanel;

    [Header("Level Loading")]
    public LevelLoader levelLoader;
    public int gameSceneIndex = 1;    

    void Start()
    {
        Time.timeScale = 1f;

        if (MainPanel != null) MainPanel.SetActive(true);
    }

    public void PlayGame()
    {
        if (levelLoader != null) levelLoader.LoadLevel(gameSceneIndex);
        else SceneManager.LoadScene(gameSceneIndex);
    }

    public void OpenCredits()
    {
        if (CreditsPanel != null) CreditsPanel.SetActive(true);
        if (MainPanel != null) MainPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        if (CreditsPanel != null) CreditsPanel.SetActive(false);
        if (MainPanel != null) MainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}