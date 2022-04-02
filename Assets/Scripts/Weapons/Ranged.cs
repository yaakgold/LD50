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

        var proj = Instantiate(projectile, transform.position, transform.rotation);
        if(proj.TryGetComponent(out Projectile p))
        {
            p.Fire(fireVelocity, damage, "Enemy");
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
