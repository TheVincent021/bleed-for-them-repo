using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health = 4;

    new Rigidbody2D rigidbody;

    public Action Damaged;
    public Action Died;

    void Awake() =>
        rigidbody = GetComponentInParent<Rigidbody2D>();

    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.CompareTag("Bullet")) 
        {
            Damage();
            Knockback(col.transform.position);
        }
    }

    void Damage ()
    {
        Damaged.Invoke();
        if (health - GameStats.Instance.gunDamage > 0)
            health -= GameStats.Instance.gunDamage;
        else
        {
            Died.Invoke();
            StartCoroutine(DestroyCollider());
        }
    }

    void Knockback(Vector3 contactPosition) 
    {
        var forceDirection = contactPosition - GameObject.FindWithTag("Barrel").transform.position;
        forceDirection.Normalize();
        rigidbody.AddForce(forceDirection * GameStats.Instance.gunKnockbackForce);
    }

    IEnumerator DestroyCollider () 
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(GetComponentInParent<CircleCollider2D>());
        rigidbody.velocity = Vector2.zero;
        Destroy(this.gameObject);
    }
}
