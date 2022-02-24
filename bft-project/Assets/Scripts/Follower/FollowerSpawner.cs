using UnityEngine;

public class FollowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject followerPrefab;

    private void Awake()
    {
        var instance = Instantiate(followerPrefab, transform.position, Quaternion.identity, transform.parent);
        instance.GetComponent<AIPathfinder>().SetTarget(GameObject.FindWithTag("Player").transform);
        Destroy(this);
    }
}