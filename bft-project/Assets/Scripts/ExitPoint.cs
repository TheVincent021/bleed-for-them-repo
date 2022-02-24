using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] bool isInteractable = false;
    public static Action LevelFinished;

    PlayerInput playerInput;

    private void Awake() =>
        playerInput = GameObject.FindObjectOfType<PlayerInput>();

    public void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.CompareTag("Player"))
            isInteractable = true;
    }

    public void OnTriggerExit2D(Collider2D col) 
    {
        if (col.CompareTag("Player"))
            isInteractable = false;
    }

    public void OnInteract() 
    {
        if (isInteractable)
        {
            GameStats.Instance.currentLevel += 1;
            if (GameObject.FindObjectOfType<FollowerHealth>() != null) GameStats.Instance.followersSaved += 1;

            LevelFinished?.Invoke();
            StartCoroutine(LoadLevelsNexus());
        }
    }

    IEnumerator LoadLevelsNexus() 
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("StatsScene");
    }
}
