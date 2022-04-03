using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    public float range;
    public float pauseTime;
    public float attackSpeed;

    public override bool CanFire()
    {
        if (!location.IsNotFiring) return false;

        return currentResetTime == resetTime;
    }

    public override void Fire()
    {
        if (!CanFire()) return;

        AudioManager.instance?.Play(ownerType + "Fire2");

        StartCoroutine(AttackFistVisual());
        StartCoroutine(Timer());
    }

    private IEnumerator AttackFistVisual()
    {
        location.IsNotFiring = false;

        while(Vector2.Distance(transform.position, transform.parent.position) < range)
        {
            transform.position += transform.up * Time.deltaTime * attackSpeed;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(pauseTime);

        while (Vector2.Distance(transform.position, transform.parent.position) > 0.1f)
        {
            transform.position -= transform.up * Time.deltaTime * attackSpeed;
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = Vector3.zero;

        location.IsNotFiring = true;
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
