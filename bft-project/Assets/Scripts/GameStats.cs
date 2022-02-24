using UnityEngine;

public class GameStats : MonoBehaviour
{
    static GameStats instance;

    [Header("Game")]
    public int currentLevel = 1;
    public int followersSaved = 0;
    public int sacrificeCount = 0;
    [Header("Player")]
    public int playerMaxHealth = 3;
    public float playerSpeed = 3f;
    [Header("Follower")]
    public int followerMaxHealth = 2;
    [Header("Gun")]
    public int gunDamage = 1;
    public float gunKnockbackForce = 200;
    public int gunClipCapacity = 6;
    public int fireRate = 10;
    public float reloadTime = 0.9166667f;
    public GameObject gunBulletType;
    public GameObject spreadBulletType;
    public GameObject gunMuzzleFlash;

    int defPlayerMaxHealth;
    float defPlayerSpeed;
    int defGunDamage;
    float defGunKnockbackForce;
    int defGunClipCapacity;
    int defGunFireRate;
    float defGunReloadTime;
    GameObject defGunMuzzleFlash;
    GameObject defGunBulletType;

    public static GameStats Instance => instance;

    private void OnEnable()
    {
        PowerUpsManager.Applied += PowerUpApplied;
        PlayerHealth.Died += Reset;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        defPlayerMaxHealth = playerMaxHealth;
        defPlayerSpeed = playerSpeed;
        defGunDamage = gunDamage;
        defGunKnockbackForce = gunKnockbackForce;
        defGunClipCapacity = gunClipCapacity;
        defGunFireRate = fireRate;
        defGunReloadTime = reloadTime;
        defGunMuzzleFlash = gunMuzzleFlash;
        defGunBulletType = gunBulletType;
    }


    void PowerUpApplied(PowerUp powerUp)
    {
        switch (powerUp)
        {
            case PowerUp.DamageBoost:
                defGunDamage += 1;
                gunDamage += 1;
                break;
            case PowerUp.SpeedBoost:
                defPlayerSpeed += 1.5f;
                playerSpeed += 1.5f;
                break;
            case PowerUp.HealthPlus:
                defPlayerMaxHealth += 1;
                playerMaxHealth += 1;
                break;
            case PowerUp.ClipCapacityPlus:
                defGunClipCapacity += 3;
                gunClipCapacity += 3;
                break;
            case PowerUp.SpreadBullet:
                gunBulletType = spreadBulletType;
                defGunBulletType = spreadBulletType;
                break;
            case PowerUp.KnockBackBoost:
                gunKnockbackForce += 200f;
                defGunKnockbackForce += 200f;
                break;
            default:
                break;
        }

    }

    public void Reset() 
    {
        currentLevel = 1;
        followersSaved = 0;
        sacrificeCount = 0;

        playerMaxHealth = defPlayerMaxHealth;
        playerSpeed = defPlayerSpeed;
        gunDamage = defGunDamage;
        gunKnockbackForce = defGunKnockbackForce;
        gunClipCapacity = defGunClipCapacity;
        fireRate = defGunFireRate;
        reloadTime = defGunReloadTime;
        gunMuzzleFlash = defGunMuzzleFlash;
        gunBulletType = defGunBulletType;
    }
}
