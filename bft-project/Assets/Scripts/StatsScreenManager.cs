using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI followerNumberText;
    [SerializeField] Animator fadeScreen;

    PlayerInput playerInput;

    private void Awake() =>
        playerInput = GameObject.FindObjectOfType<PlayerInput>();

    private void Start()
    {
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap("UIPopUp");

        levelNumberText.text = GameStats.Instance.currentLevel.ToString();
        followerNumberText.text = GameStats.Instance.followersSaved.ToString();
    }

    public void OnSubmit() =>
        StartCoroutine(LoadNextLevel());

    IEnumerator LoadNextLevel() 
    {
        playerInput.DeactivateInput();
        fadeScreen.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap("Default");
        SceneManager.LoadScene(GameStats.Instance.currentLevel + 1);
    }
}
