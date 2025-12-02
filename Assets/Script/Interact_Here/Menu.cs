using UnityEngine;
using UnityEngine.SceneManagement; 

public class Menu : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    private void Start()
    {
        SoundManager.Instance.PlayBGM("Background");
    }
    public void PlayGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX("Click");

        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX("Click");

        Debug.Log("ออกเกม");
        Application.Quit();
    }
}