using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponLocation : MonoBehaviour
{
    [SerializeField] float maxDist;
    [SerializeField] float rotSpeed;

    public bool IsNotFiring { get; set; } = true;

    Weapon weapon;
    Vector3 difference;

    private void Start()
    {
        //TODO: Change this
        weapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsNotFiring)
        {
            //difference = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
            //difference.Normalize();
            //float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            ////TODO: Put in some slerping or something like that
            //transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.Euler(0f, 0f, (rotation_z + maxDist)), Time.deltaTime * rotSpeed);
        }
    }

    public float GetMaxDist() {return maxDist; }

    public void Fire()
    {
        weapon.Fire();
    }
}
