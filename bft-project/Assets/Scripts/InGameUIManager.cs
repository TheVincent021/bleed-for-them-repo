using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] Animator fadeScreen;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Transform playerHeartsHolder;
    [SerializeField] GameObject playerHeartPrefab;
    Stack<GameObject> playerHearts;
    [SerializeField] Transform followerHeartsHolder;
    [SerializeField] GameObject followerHeartPrefab;
    Stack<GameObject> followerHearts;
    [SerializeField] Transform bulletsHolder;
    [SerializeField] GameObject bulletPrefab;
    Stack<GameObject> bullets;
    [SerializeField] UIElementTween powerUpCardHolder;
    [SerializeField] Image powerUpCardRenderer;
    [SerializeField] Sprite[] powerUpCards;

    private void OnEnable()
    {
        PlayerHealth.Damaged += PopPlayerHeart;
        PlayerHealth.Died += GameOverPanel;
        FollowerHealth.Damaged += PopFollowerHeart;
        Gun.Shot += PopBullet;
        Gun.Reloaded += FillGunClip;
        PowerUpsManager.Activated += PowerUpActivated;
        PowerUpsManager.Applied += PowerUpApplied;
        ExitPoint.LevelFinished += Fade;
    }

    private void OnDisable()
    {
        PlayerHealth.Damaged -= PopPlayerHeart;
        PlayerHealth.Died -= GameOverPanel;
        FollowerHealth.Damaged -= PopFollowerHeart;
        Gun.Shot -= PopBullet;
        Gun.Reloaded -= FillGunClip;
        PowerUpsManager.Activated -= PowerUpActivated;
        PowerUpsManager.Applied -= PowerUpApplied;
        ExitPoint.LevelFinished -= Fade;
    }

    private void Awake()
    {
        playerHearts = new Stack<GameObject>();
        followerHearts = new Stack<GameObject>();
        bullets = new Stack<GameObject>();
        FillPlayerHearts();
        FillFollowerHearts();
        FillGunClip();
    }

    void Fade() =>
        fadeScreen.SetTrigger("Fade");

    void FillPlayerHearts()
    {
        while (playerHearts.Count > 0)
            Destroy(playerHearts.Pop());
        for (int i = 0; i < GameStats.Instance.playerMaxHealth; i++)
            playerHearts.Push(Instantiate(playerHeartPrefab, playerHeartsHolder));
    }

    void PopPlayerHeart() =>
        Destroy(playerHearts?.Pop());

    void FillFollowerHearts()
    {
        while (followerHearts.Count > 0)
            Destroy(followerHearts.Pop());
        for (int i = 0; i < GameStats.Instance.followerMaxHealth; i++) 
            followerHearts.Push(Instantiate(followerHeartPrefab, followerHeartsHolder));
    }

    void PopFollowerHeart() =>
        Destroy(followerHearts?.Pop());

    void FillGunClip()
    {
        while (bullets.Count > 0)
            Destroy(bullets.Pop());
        for (int i = 0; i < GameStats.Instance.gunClipCapacity; i++)
            bullets.Push(Instantiate(bulletPrefab, bulletsHolder));
    }

    void PopBullet() =>
            Destroy(bullets?.Pop());

    void GameOverPanel()
    {
        mainPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    void PowerUpActivated(PowerUp powerUp)
    {
        mainPanel.SetActive(false);
        var i = ((int)powerUp);
        powerUpCardRenderer.sprite = powerUpCards[i];
        powerUpCardHolder.NextPosition();
    }

    void PowerUpApplied(PowerUp powerUp)
    {
        switch (powerUp)
        {
            case PowerUp.HealthPlus:
                FillPlayerHearts();
                break;
            case PowerUp.ClipCapacityPlus:
                FillGunClip();
                break;
            default:
                break;
        }

        powerUpCardHolder.NextPosition();
        mainPanel.SetActive(true);
    }
}
