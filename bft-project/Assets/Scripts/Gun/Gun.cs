using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] bool isReady = true;
    [SerializeField] bool isReloading = false;
    [SerializeField] Transform barrelEnd;
    [SerializeField] int currentAmmo = 0;

    public static Action Shot;
    public static Action Reloaded;

    public bool IsReady => isReady;
    public bool IsReloading => isReloading;

    private void OnEnable()
    {
        PowerUpsManager.Applied += PowerUpApplied;
        PlayerHealth.Died += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PowerUpsManager.Applied -= PowerUpApplied;
        PlayerHealth.Died -= OnPlayerDeath;
    }

    void Start() =>
        currentAmmo = GameStats.Instance.gunClipCapacity;

    // INPUT
    void OnShoot() 
    {
        if (isReady) 
        {
            if (currentAmmo > 0) 
                Shoot();
            else 
                StartCoroutine(StartReload());
        }
    }

    void Shoot() 
    {
        Shot?.Invoke();
        currentAmmo--;
        Instantiate(GameStats.Instance.gunMuzzleFlash, barrelEnd.position, barrelEnd.rotation);
        Instantiate(GameStats.Instance.gunBulletType, barrelEnd.position, barrelEnd.rotation);
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown() 
    {
        isReady = false;
        yield return new WaitForSeconds(1f/GameStats.Instance.fireRate);
        isReady = true;
    }

    // INPUT
    void OnReload() =>
        StartCoroutine(StartReload());

    IEnumerator StartReload() 
    {
        if (isReady && currentAmmo < GameStats.Instance.gunClipCapacity) 
        {
            isReady = false;
            isReloading = true;
            yield return new WaitForSeconds(GameStats.Instance.reloadTime);
            Reloaded?.Invoke();
            currentAmmo = GameStats.Instance.gunClipCapacity;
            isReady = true;
            isReloading = false;
        }
    }

    void PowerUpApplied(PowerUp powerUp)
    {
        if (powerUp == PowerUp.ClipCapacityPlus)
            StartCoroutine(StartReload());
    }

    void OnPlayerDeath() =>
        Destroy(this.gameObject);
}
