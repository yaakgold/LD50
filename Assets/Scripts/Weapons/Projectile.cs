using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string tagToHit;
    public float timeTillDeath;

    private int damage;

    private void Start()
    {
        Destroy(gameObject, timeTillDeath);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(tagToHit))
        {
            if (collision.TryGetComponent(out Health health))
            {
                health.DoDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    public void Fire(float speed, int dmg)
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;

        damage = dmg;
    }
}
