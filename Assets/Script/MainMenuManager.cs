using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (MainPanel != null) MainPanel.SetActive(true);
        if (CreditsPanel != null) CreditsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        if (levelLoader != null) levelLoader.LoadLevel(gameSceneIndex);
        else SceneManager.LoadScene(gameSceneIndex);
    }

    public void OpenCredits()
    {
        if (CreditsPanel != null) CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (CreditsPanel != null) CreditsPanel.SetActive(false);

        StartCoroutine(ClearButtonSelection());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator ClearButtonSelection()
    {
        yield return null; 
        EventSystem.current.SetSelectedGameObject(null); 
    }
}