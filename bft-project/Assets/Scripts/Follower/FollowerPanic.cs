using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIPathfinder))]
public class FollowerPanic : MonoBehaviour
{
    [SerializeField] bool isPanicking = false;
    [SerializeField] int treshold = 5;
    [SerializeField] float panicTime = 5f;
    [SerializeField] float freezeTime = 5f;
    [SerializeField] Transform bait;

    int panicPoint = 0;
    Vector3 direction;

    public static Action Panicked;


    System.Random random;
    new Transform transform;
    AIPathfinder pathfinder;

    void OnEnable()
    {
        FollowerHealth.Damaged += () => AddPanicPoint(2);
        PlayerHealth.Damaged += () => AddPanicPoint(1);
    }

    void Awake()
    {
        random = new System.Random();

        transform = base.transform;
        pathfinder = GetComponent<AIPathfinder>();
    }

    void AddPanicPoint(int amount)
    {
        panicPoint += amount;
        if (panicPoint >= treshold && !isPanicking)
        {
            StartCoroutine(EndPanic());
            panicPoint = treshold;
            Panicked?.Invoke();
            isPanicking = true;
            ChooseRandomDirection();

            pathfinder.SetTarget(bait);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isPanicking && !col.transform.CompareTag("Player") && !col.transform.CompareTag("Follower"))
            ChooseRandomDirection();
    }

    void ChooseRandomDirection()
    {
        var newDirection =
            new Vector3(random.Next(-1, 2), random.Next(-1, 2), 0f);
        if (newDirection == Vector3.zero || newDirection == direction
            || newDirection.x == direction.x || newDirection.y == direction.y)
        {
            ChooseRandomDirection();
            return;
        }
        direction = newDirection;
        bait.position = transform.position + direction;
    }

    IEnumerator EndPanic()
    {
        yield return new WaitForSeconds(panicTime);
        isPanicking = false;
        pathfinder.SetTarget(transform);
        yield return new WaitForSeconds(freezeTime);
        pathfinder.SetTarget(GameObject.FindWithTag("Player").transform);
        panicPoint = 0;
    }
}
