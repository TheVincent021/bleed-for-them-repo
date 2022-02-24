using System;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] bool isClosed = false;
    [SerializeField] Transform[] doors;
    [SerializeField] EnemyHealth[] enemies;
    bool playerPassed = false;
    bool followerPassed = false;

    private void Update()
    {
        if (isClosed)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                    return;
            }
            ChangeDoorsState(true);
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerPassed = true;
        else if (other.CompareTag("Follower")) followerPassed = true;

        if (playerPassed && followerPassed) ChangeDoorsState(false);
    }

    void ChangeDoorsState(bool state)
    {
        isClosed = true;
        for (int i = 0; i < doors.Length; i++)
            doors[i].gameObject.SetActive(!state);
    }
}
