using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Vector2 movementInput;
    Vector2 movement;

    new Rigidbody2D rigidbody;
    PlayerInput playerInput;

    private void OnEnable() =>
        PlayerHealth.Died += OnDeath;

    private void OnDisable() =>
        PlayerHealth.Died -= OnDeath;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        movementInput = playerInput.actions["movement"].ReadValue<Vector2>();
        movement = movementInput * GameStats.Instance.playerSpeed;
        rigidbody.velocity = movement;
    }

    void OnDeath()
    {
        rigidbody.Sleep();
        this.enabled = false;
    }
}
