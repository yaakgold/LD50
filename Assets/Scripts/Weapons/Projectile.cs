using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string tagToHit;

    private int damage;

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

    public void Fire(float speed, int dmg, string tag)
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;

        damage = dmg;
        tagToHit = tag;
    }
}
