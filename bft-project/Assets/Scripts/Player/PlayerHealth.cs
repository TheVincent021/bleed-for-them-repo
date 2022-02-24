using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] int health = 3;
    [SerializeField] float recoveryTime = 1f;
    [SerializeField] bool isRecovering = false;

    public static Action Damaged;
    public static Action Died;

    public bool IsRecovering => isRecovering;

    private void OnEnable() =>
        PowerUpsManager.Applied += PowerUpApplied;

    private void OnDisable() =>
        PowerUpsManager.Applied -= PowerUpApplied;

    private void Awake() =>
        health = GameStats.Instance.playerMaxHealth;
    

    public void Hit(int damage) 
    {
        if (!isRecovering) 
        {
            if (health > 1) 
            {
                StartCoroutine(Recovery());
                health--;
            }
            else Die();
            Damaged.Invoke();
        }
    }

    IEnumerator Recovery() 
    {
        isRecovering = true;
        yield return new WaitForSeconds(recoveryTime);
        isRecovering = false;
    }

    void Die() 
    {
        Died.Invoke();
        Destroy(GetComponent<CircleCollider2D>());
        GameObject.FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("GameOver");
        Destroy(this);
    }

    void PowerUpApplied(PowerUp powerUp)
    {
        if (powerUp == PowerUp.HealthPlus)
            health = GameStats.Instance.playerMaxHealth;
    }
}
