using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(Rigidbody2D))]
public class AIPathfinder : MonoBehaviour
{
    [SerializeField] bool isEnabled = true;
    [SerializeField] bool isStopped = false;
    [SerializeField] Transform target;
    [SerializeField] float maxSpeed = 200f;
    [SerializeField] float accelerateRate = 10f;
    [SerializeField] float normalDrag = 1f;
    [SerializeField] float slowdownDrag = 2f;
    [SerializeField] float slowdownDistance = 1.5f;
    [SerializeField] float stopDistance = 0.5f;
    [SerializeField] float nextWaypointDistance = 3f;
    [SerializeField] float pathUpdateRate = 10f;
    [SerializeField] float followSmoothtime = 0.5f;

    Vector2 direction = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Path currentPath;
    int currentWaypoint;

    Seeker seeker;
    new Rigidbody2D rigidbody;

    public bool IsEnabled => isEnabled;
    public bool IsStopped => isStopped;
    public Transform Target => target;
    public void SetTarget(Transform value) => target = value;

    private void Awake ()
    {
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine("UpdatePath");
        StartCoroutine("Accelerate");
    }

    IEnumerator UpdatePath()
    {
        while(true)
        {
            if (seeker.IsDone() && target != null)
                seeker.StartPath(rigidbody.position, target.position, OnPathComplete);
            else if (target == null)
                rigidbody.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f/pathUpdateRate);
        }
    }

    private void FixedUpdate ()
    {
        if (currentPath == null || currentWaypoint >= currentPath.vectorPath.Count)
            return;

        direction = ((Vector2)currentPath.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        var distance = Vector2.Distance(rigidbody.position, currentPath.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    IEnumerator Accelerate()
    {
        while(true)
        {
            if (target != null)
            {
                var newDirection = direction;
                var distance = Vector2.Distance(rigidbody.position, target.position);

                if (distance < slowdownDistance)
                    rigidbody.drag = slowdownDrag;
                else
                    rigidbody.drag = normalDrag;

                if (distance < stopDistance || isEnabled == false)
                {
                    newDirection = Vector2.zero;
                    rigidbody.velocity = Vector2.zero;
                    isStopped = true;
                }
                else isStopped = false;

                if (!isStopped) rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, newDirection * maxSpeed, ref velocity, followSmoothtime);
            }

            yield return new WaitForSeconds(1f/accelerateRate);
        }
    }

    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }

    public void Enable() =>
        isEnabled = true;

    public void Disable() =>
        isEnabled = false; 
}
