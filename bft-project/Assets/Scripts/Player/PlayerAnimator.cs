using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    Vector2 movementInput;
    Vector2 movementDirection;
    Vector2 mouseDirection;

    Animator animator;
    new Transform transform;

    private void OnEnable()
    {
        PlayerHealth.Damaged += OnHit;
        PlayerHealth.Died += OnDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.Damaged -= OnHit;
        PlayerHealth.Died -= OnDeath;
    }

    void Awake() 
    {
        animator = GetComponent<Animator>();
        transform = base.transform;
    }

    void Update() 
    {
        movementDirection = new Vector2(Math.Sign(movementInput.x), Math.Sign(movementInput.y));

        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var mouseDistance = ((Vector2)(mousePos - transform.position)).normalized;
        if (mouseDistance.y > -0.5f && mouseDistance.y < 0.5f && mouseDistance.x > 0f) mouseDirection = Vector2.right;
        else if (mouseDistance.y > -0.5f && mouseDistance.y < 0.5f && mouseDistance.x < 0f) mouseDirection = Vector2.left;
        if (mouseDistance.x > -0.5f && mouseDistance.x < 0.5f && mouseDistance.y > 0f) mouseDirection = Vector2.up;
        else if (mouseDistance.x > -0.5f && mouseDistance.x < 0.5f && mouseDistance.y < 0f) mouseDirection = Vector2.down;

        animator.SetFloat("MoveX", movementDirection.x);
        animator.SetFloat("MoveY", movementDirection.y);
        animator.SetFloat("MouseX", mouseDirection.x);
        animator.SetFloat("MouseY", mouseDirection.y);

        if (movementInput.x != 0f)
        {
            if (mouseDistance.x > 0f)
                mouseDirection = Vector2.right;
            else if (mouseDistance.x < 0f)
                mouseDirection = Vector2.left;
        }
        else if (movementInput.y != 0f)
        {
            if (mouseDistance.y > 0f)
                mouseDirection = Vector2.up;
            else if (mouseDistance.y < 0f)
                mouseDirection = Vector2.down;
            
        }

        animator.SetFloat("Backwards", movementDirection == -mouseDirection ? 1f : 0f);
    }

    public void OnMovement(InputValue value)
    {
        var currentInput = value.Get<Vector2>();
        if (movementInput.x != 0f)
        {
            if (currentInput.y != 0f)
                movementInput = new Vector2(0f, currentInput.y);
        }
        else if (movementInput.y != 0f)
        {
            if (currentInput.x != 0f)
                movementInput = new Vector2(currentInput.x, 0f);
        }
        else
            movementInput = currentInput;
    }

    public void OnMovementRelease() =>
        movementInput = Vector2.zero;

    void OnHit() =>
        animator.SetTrigger("Damage");

    void OnDeath()
    {
        animator.SetTrigger("Die");
        this.enabled = false;
    }
}
