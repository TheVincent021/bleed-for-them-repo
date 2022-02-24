using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] float magnitude = 1f;

    new Transform transform;

    private void OnEnable()
    {
        PlayerHealth.Damaged += DamageShake;
        Gun.Shot += ShotShake;
    }

    private void OnDisable()
    {
        PlayerHealth.Damaged -= DamageShake;
        Gun.Shot -= ShotShake;
    }

    private void Awake() =>
        transform = base.transform;

    void DamageShake() =>
        StartCoroutine(StartShake(0.15f, 0.15f));

    void ShotShake() =>
        StartCoroutine(StartShake(0.1f, 0.1f));

    IEnumerator StartShake(float magnitude, float duration) 
    {
        Vector3 originPosition = transform.localPosition;

        float elapsed = 0f;

        if (magnitude == -1f) magnitude = this.magnitude;
        if (duration == -1f) duration = this.duration;

        while (elapsed < duration) 
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originPosition;
    }
}
