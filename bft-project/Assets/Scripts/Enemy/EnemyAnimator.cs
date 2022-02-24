using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    Transform target;
    Animator animator;
    SpriteRenderer graphics;
    AIPathfinder pathfinder;
    EnemyAttack attack;

    private void OnEnable()
    {
        var health = transform.parent.GetComponentInChildren<EnemyHealth>();
        health.Damaged += () => animator.SetTrigger("Damage");
        health.Died += delegate () { animator.SetTrigger("Died"); graphics.sortingLayerName = "Below"; this.enabled = false; };
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        graphics = GetComponent<SpriteRenderer>();
        pathfinder = GetComponentInParent<AIPathfinder>();
        attack = GetComponentInParent<EnemyAttack>();
    }

    void Update()
    {
        target = attack.GetTarget();

        animator.SetBool("IsMoving", target != null);

        if (animator.GetBool("IsMoving"))
        {
            var direction = (target.position - transform.position).normalized;
            if (direction.x > 0f && direction.x > Mathf.Abs(direction.y))
            {
                animator.Play("MoveRight");
            }
            if (direction.y > 0f && direction.y > Mathf.Abs(direction.x))
            {
                animator.Play("MoveUp");
            }
            if (direction.x < 0f && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animator.Play("MoveLeft");
            }
            if (direction.y < 0f && Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                animator.Play("MoveDown");
            }
        }
    }
}
