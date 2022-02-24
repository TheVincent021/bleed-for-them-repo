using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PowerUp
{
    DamageBoost, 
    SpeedBoost, 
    HealthPlus, 
    ClipCapacityPlus, 
    SpreadBullet, 
    KnockBackBoost
}

public class PowerUpsManager : MonoBehaviour
{
    PowerUp newPowerUp;
    static List<PowerUp> appliedPowerUps;

    public static Action<PowerUp> Activated;
    public static Action<PowerUp> Applied;
    System.Random random;

    PlayerInput playerInput;

    private void OnEnable() =>
        Altar.Activated += Activate;

    private void OnDisable() =>
        Altar.Activated -= Activate;

    private void Awake()
    {
        if (appliedPowerUps == null)
            appliedPowerUps = new List<PowerUp>();
        random = new System.Random();
        playerInput = GameObject.FindObjectOfType<PlayerInput>(); 
    }

    void Activate() =>
        StartCoroutine(StartActivating());

    IEnumerator StartActivating()
    {
        playerInput.DeactivateInput();

        yield return new WaitForSeconds(2f);

        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap("UIPopUp");
        playerInput.currentActionMap.FindAction("Submit").performed += Apply;

        newPowerUp = RandomPowerUp();
        Activated.Invoke(newPowerUp);
    }

    PowerUp RandomPowerUp()
    {
        var powerUp = PowerUp.DamageBoost;
        do
        {
            Array values = Enum.GetValues(typeof(PowerUp));
            powerUp = (PowerUp)values.GetValue(random.Next(values.Length));
        } while (appliedPowerUps.Contains(powerUp));

        appliedPowerUps.Add(powerUp);

        return powerUp;
    }

    void Apply(InputAction.CallbackContext ctx)
    {
        playerInput.currentActionMap.FindAction("Submit").performed -= Apply;
        Applied.Invoke(newPowerUp);
        playerInput.SwitchCurrentActionMap("Default");
    }
}