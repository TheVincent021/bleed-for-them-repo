using UnityEngine;
using UnityEngine.InputSystem;

public class GunAnimator : MonoBehaviour
{
    [SerializeField] Vector2 mouseDirection;
    [SerializeField] Transform barrelEnd;
    [SerializeField] Vector3[] barrelEndPositions;

    Animator animator;
    SpriteRenderer graphics;
    Transform player;
    Gun gun;

    private void OnEnable() =>
        Gun.Shot += Shoot;

    private void OnDisable() =>
        Gun.Shot -= Shoot;

    void Awake() 
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        graphics = GetComponent<SpriteRenderer>();
        gun = GetComponent<Gun>();
    }

    void Update() 
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var mouseDistance = ((Vector2)(mousePos - player.position)).normalized;
        if (mouseDistance.y > -0.5f && mouseDistance.y < 0.5f && mouseDistance.x > 0f) mouseDirection = Vector2.right;
        else if (mouseDistance.y > -0.5f && mouseDistance.y < 0.5f && mouseDistance.x < 0f) mouseDirection = Vector2.left;
        if (mouseDistance.x > -0.5f && mouseDistance.x < 0.5f && mouseDistance.y > 0f) mouseDirection = Vector2.up;
        else if (mouseDistance.x > -0.5f && mouseDistance.x < 0.5f && mouseDistance.y < 0f) mouseDirection = Vector2.down;

        animator.SetBool("IsHorizontal", mouseDirection.y == 0f);
        animator.SetBool("Reload", gun.IsReloading);

        graphics.flipY = mouseDirection.x == -1f ? true : false;
        graphics.sortingOrder = mouseDirection.y == 1f ? -1 : 1;

        FixBarrelEndPosition();
    }

    void FixBarrelEndPosition()
    {
        if (mouseDirection.x == 0f) 
        {
            barrelEnd.localPosition = barrelEndPositions[0];
        } 
        else if (mouseDirection.x == -1f) 
        {
            barrelEnd.localPosition = barrelEndPositions[1];
        } 
        else if (mouseDirection.x == 1f) 
        {
           barrelEnd.localPosition = barrelEndPositions[2];
        }
    }

    void Shoot() =>
            animator.SetTrigger("Shoot");
}
