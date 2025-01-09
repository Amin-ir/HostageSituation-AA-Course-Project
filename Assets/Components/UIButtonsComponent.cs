using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonsComponent : MonoBehaviour
{
    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        RevertTimeScale();
        FadeAndSwitchScene(currentScene.name);
    }

    public void LoadMainMenu()
    {
        RevertTimeScale();
        FadeAndSwitchScene("Menu");
    }

    void RevertTimeScale()
    {
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        FadeAndSwitchScene("SampleScene");
    }

    public void FadeAndSwitchScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        var fadingObject = FindObjectOfType<FadingComponent>();
        fadingObject.Unfade();

        yield return new WaitUntil(() => Mathf.Abs(fadingObject.imageAlpha - 1) < 0.01f);

        SceneManager.LoadScene(sceneName);
    }

}
