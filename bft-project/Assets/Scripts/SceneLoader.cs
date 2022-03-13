using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator fadeScreen;

    public void LoadScene(int index)
    {
        StartCoroutine(FadeLoad(index));
    }


    public void LoadScene(string name)
    {
        StartCoroutine(FadeLoad(name));
    }


    public void LoadCurrentLevel()
    {
        StartCoroutine(FadeLoad(GameStats.Instance.currentLevel + 1));
    }


    public void LoadNextScene()
    {
        StartCoroutine(FadeLoad(SceneManager.GetActiveScene().buildIndex + 1));
    }


    public void ReloadScene()
    {
        StartCoroutine(FadeLoad(SceneManager.GetActiveScene().buildIndex));
    }

    public void ExitApplication()
    {
        StartCoroutine(FadeExit());
    }

    IEnumerator FadeLoad(int index)
    {
        fadeScreen.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(index);
    }

    IEnumerator FadeLoad(string name)
    {
        fadeScreen.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }

    IEnumerator FadeExit()
    {
        fadeScreen.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
