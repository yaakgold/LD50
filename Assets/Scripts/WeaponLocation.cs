using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponLocation : MonoBehaviour
{
    [SerializeField] float maxDist;

    public bool IsNotFiring { get; set; } = true;

    [SerializeField] Weapon weapon;
    Vector3 difference;

    private void Start()
    {
        //TODO: Change this
        weapon = GetComponentInChildren<Weapon>();
    }

    public float GetMaxDist() {return maxDist; }

    public Weapon GetWeapon() { return weapon; }
    public void SetWeapon(Weapon wpn) { weapon = wpn; }

    public void Fire()
    {
        if(weapon != null)
            weapon.Fire();
    }
}
