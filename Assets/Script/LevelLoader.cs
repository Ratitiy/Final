using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("UI References")]
    public GameObject loadingScreenPanel;
    public Slider loadingSlider;

    [Header("Settings")]
    public float visualLoadingSpeed = 0.5f;

    public void LoadLevel(int sceneIndex)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreenPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        float currentVisualProgress = 0f; 

        while (!operation.isDone)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            currentVisualProgress = Mathf.MoveTowards(currentVisualProgress, targetProgress, visualLoadingSpeed * Time.unscaledDeltaTime);

            if (loadingSlider != null)
            {
                loadingSlider.value = currentVisualProgress;
            }
            if (currentVisualProgress >= 1f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}