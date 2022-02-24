using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIPathfinder))]
public class FollowerHealth : MonoBehaviour, IHealth
{
    [SerializeField] int health = 3;
    [SerializeField] float recoveryTime = 1f;
    [SerializeField] bool isRecovering = false;

    public static Action Damaged;
    public static Action<bool> Died;

    private void OnEnable() => 
        Altar.Activated += Sacrifice;

    private void OnDisable() =>
        Altar.Activated -= Sacrifice;

    void Awake() =>
        GameStats.Instance.followerMaxHealth = health;

    public void Hit(int damage) 
    {
        if (!isRecovering) 
            Damage();
    }

    void Damage() 
    {
        health--;
        Damaged?.Invoke();

        if (health > 0)
            StartCoroutine(Recovery());
        else
            Die(false);
    }

    IEnumerator Recovery() 
    {
        isRecovering = true;
        yield return new WaitForSeconds(recoveryTime);
        isRecovering = false;
    }

    void Sacrifice() =>
        Die(true);

    void Die(bool sacrificed) 
    {
        Died?.Invoke(sacrificed);
        GetComponent<AIPathfinder>().Disable();
        var collider = GetComponent<Collider2D>();
        if (collider) collider.enabled = false;
        Destroy(this);
    }
}
