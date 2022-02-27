using Pathfinding;
using UnityEngine;

public class FollowerAnimator : MonoBehaviour
{
    [SerializeField] float animateThreshold = 0.9f;

    Animator animator;
    SpriteRenderer graphics;
    AIPathfinder pathfinder;
    new Rigidbody2D rigidbody;

    Transform player;

    private void OnEnable()
    {
        FollowerHealth.Damaged += Damaged;
        FollowerHealth.Died += Died;
    }

    private void OnDisable()
    {
        FollowerHealth.Damaged -= Damaged;
        FollowerHealth.Died -= Died;
        graphics.sortingLayerName = "Below";
    }

    void Awake() 
    {
        animator = GetComponent<Animator>();
        graphics = GetComponent<SpriteRenderer>();
        pathfinder = GetComponentInParent<AIPathfinder>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update() 
    {
        animator.SetBool("isMoving", !pathfinder.IsStopped);

        var velocityX = rigidbody.velocity.x;
        var velocityY = rigidbody.velocity.y;
        if (!pathfinder.IsStopped)
        {
            if (velocityX > animateThreshold && velocityX > Mathf.Abs(velocityY))
            {
                animator.Play("MoveRight");
            }
            if (velocityY > animateThreshold && velocityY > Mathf.Abs(velocityX))
            {
                animator.Play("MoveUp");
            }
            if (velocityX < animateThreshold && Mathf.Abs(velocityX) > Mathf.Abs(velocityY))
            {
                animator.Play("MoveLeft");
            }
            if (velocityY < animateThreshold && Mathf.Abs(velocityY) > Mathf.Abs(velocityX))
            {
                animator.Play("MoveDown");
            }
        }
    }

    void Damaged() =>
        animator.SetTrigger("Damage");

    void Died(bool sacrificed)
    {
        animator.SetTrigger(sacrificed ? "Impale" : "Die"); 
        this.enabled = false;
    }
}
