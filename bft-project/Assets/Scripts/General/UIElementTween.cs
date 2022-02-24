using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIElementTween : MonoBehaviour
{
    [SerializeField] Vector3[] positions;
    [SerializeField] [Range(0f, 1f)] float smoothTime = 0.5f;
    
    int targetPositionIndex = 0;
    Vector3 velocity;

    new RectTransform transform;

    void Awake () =>
        transform = GetComponent<RectTransform>();

   void Update () =>
       transform.localPosition = Vector3.SmoothDamp(transform.localPosition, positions[targetPositionIndex], ref velocity, smoothTime);

   public void NextPosition() =>
        targetPositionIndex = targetPositionIndex < positions.Length - 1 ? targetPositionIndex + 1 : 0;
}
