using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Level Loading")]
    public LevelLoader levelLoader;
    public int gameSceneIndex = 1;

    [Header("Audio Settings")]
    public AudioMixer mainMixer;     
    public Slider musicSlider;       
    public Slider sfxSlider;         

    void Start()
    {
        Time.timeScale = 1f;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("SavedMusicVol", 1f);
            musicSlider.onValueChanged.AddListener(SetMusicVolume); 
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SavedSFXVol", 1f);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume); 
        }
    }

    public void PlayGame()
    {
        if (levelLoader != null) levelLoader.LoadLevel(gameSceneIndex);
        else SceneManager.LoadScene(gameSceneIndex);
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("SavedMusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("SavedSFXVol", value);
    }
}