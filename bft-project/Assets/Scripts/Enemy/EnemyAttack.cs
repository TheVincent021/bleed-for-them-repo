using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPathfinder))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isDisabled = false;
    [SerializeField] int damage = 1;
    [SerializeField] float attackRange = 0.2f;
    [SerializeField] List<Transform> targets;
    [SerializeField] Transform currentTarget;

    AIPathfinder pathfinder;

    public bool IsDisabled => isDisabled;
    public bool IsAttacking => isAttacking;
    public Transform GetTarget() => currentTarget;
    public void Enable() => isDisabled = false;
    public void Disable() => isDisabled = true;

    private void OnEnable()
    {
        var health = GetComponentInChildren<EnemyHealth>();
        health.Damaged += delegate () { if (!isDisabled) isAttacking = true; };
        health.Died += delegate () { isDisabled = true; isAttacking = false; };
    }

    void Awake() 
    {
        pathfinder = GetComponent<AIPathfinder>();
        pathfinder.Disable();
    }

    void Update() 
    {
        if (isAttacking && currentTarget != null) 
        {
            ChooseTarget();
            Attack();
            CheckTargets();
        } 
        else
        {
            pathfinder.SetTarget(null);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<IHealth>() != null && !isDisabled)
        {
            if (targets.Contains(col.transform))
                return;

            isAttacking = true;
            targets.Add(col.transform);
            if (currentTarget == null)
            {
                currentTarget = targets[0];
            }
            pathfinder.Enable();
        }
    }

    void ChooseTarget() 
    {
        if (targets.Count == 0)
        {
            currentTarget = null;
            return;
        }

        if (targets.Count == 1)
        {
            currentTarget = targets[0];
            pathfinder.SetTarget(targets[0]);
            return;
        }

        var smallestDistance = Vector3.Distance(transform.position, currentTarget.position);
        for (int i = 0; i < targets.Count; i++) 
        {
            var distance = Vector3.Distance(transform.position, targets[i].position);
            if (distance < smallestDistance && targets[i].GetComponent<IHealth>() != null) 
            {
                smallestDistance = distance;
                currentTarget = targets[i];
            }
        }
        pathfinder.SetTarget(currentTarget);
    }

    void Attack () {
        if (currentTarget?.GetComponent<IHealth>() != null) 
        {
            if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
                currentTarget.GetComponent<IHealth>().Hit(damage);
        }
    }

    void CheckTargets () 
    {
        for (int i = 0; i < targets.Count; i++) 
        {
            if (targets[i].GetComponent<IHealth>() == null)
                targets.RemoveAt(i);
        }

        if (targets.Count == 0)
            pathfinder.Disable();
    }
 }
