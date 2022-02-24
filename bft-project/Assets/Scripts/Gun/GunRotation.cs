using UnityEngine;
using UnityEngine.InputSystem;

public class GunRotation : MonoBehaviour
{
    [SerializeField] [Range(0f, 180f)] float offset = 90f;
    Vector3 difference;

    new Transform transform;

    void Awake () =>
         transform = base.transform;

    void Update()
    {
        difference = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, -rotZ + offset);
    }
}
