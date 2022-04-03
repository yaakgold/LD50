using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Weapon
{
    public GameObject projectile;
    public float fireVelocity;

    public override bool CanFire()
    {
        if (!location.IsNotFiring) return false;

        return currentResetTime == resetTime;
    }

    public override void Fire()
    {
        if (!CanFire()) return;

        AudioManager.instance?.Play(ownerType + "Fire1");

        var proj = Instantiate(projectile, transform.position, transform.rotation);
        if(proj.TryGetComponent(out Projectile p))
        {
            if (location.GetComponentInParent<PlayerController>() != null)
            {
                damage = (int)(location.GetComponentInParent<PlayerController>().healthDamageMult * defaultDamage);
            }

            p.Fire(fireVelocity, damage);
        }
        
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while(currentResetTime > 0)
        {
            currentResetTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        currentResetTime = resetTime;
    }
}
