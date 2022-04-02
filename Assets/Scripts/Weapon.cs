using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float resetTime;
    protected float currentResetTime;
    protected WeaponLocation location;

    private void Start()
    {
        location = GetComponentInParent<WeaponLocation>();

        currentResetTime = resetTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!location || location.IsNotFiring) return;

        if (collision.TryGetComponent(out Health health))
        {
            health.DoDamage(damage);
        }
    }

    public abstract void Fire();
    public abstract bool CanFire();
}
