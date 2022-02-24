using System;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] bool isInteractable = false;
    public static Action Activated;

    void OnTriggerEnter2D (Collider2D col) {
        if (col.CompareTag("Follower"))
            isInteractable = true;
    }

    void OnTriggerExit2D (Collider2D col) {
        if (col.CompareTag("Follower"))
            isInteractable = false;
    }

    // INPUT
    void OnInteract ()
    {
        if (isInteractable)
        {
            GameStats.Instance.sacrificeCount += 1;
            Activated?.Invoke();
        }
    }
}
