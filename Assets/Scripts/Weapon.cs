using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public bool isRanged = false;
    public float resetTime;
    protected float currentResetTime;
    protected WeaponLocation location;
    protected int defaultDamage;
    [SerializeField] protected string hitTag;
    [SerializeField] protected string ownerType;

    private void Start()
    {
        location = GetComponentInParent<WeaponLocation>();

        currentResetTime = resetTime;

        defaultDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!location || location.IsNotFiring) return;

        if (collision.TryGetComponent(out Health health) && collision.CompareTag(hitTag))
        {
            if(location.GetComponentInParent<PlayerController>() != null)
            {
                damage = (int)(location.GetComponentInParent<PlayerController>().healthDamageMult * defaultDamage);
            }

            health.DoDamage(damage);
        }
    }

    public abstract void Fire();
    public abstract bool CanFire();
}
