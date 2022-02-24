using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity, transform.parent);
        Destroy(this);
    }
}
