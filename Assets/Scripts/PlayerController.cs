using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] WeaponLocation weaponLocationLeft, weaponLocationRight;

    [SerializeField] float maxDist;
    [SerializeField] float rotSpeed;

    Vector2 movement;
    Vector3 difference;

    // Update is called once per frame
    void Update()
    {
        //Movement
        transform.position += (Vector3)movement * Time.deltaTime * speed;

        //Rotation
        difference = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        //TODO: Put in some slerping or something like that
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (rotation_z + maxDist - 90)), Time.deltaTime * rotSpeed);
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        weaponLocationLeft.Fire();
    }

    public void OnFire1(InputValue value)
    {
        weaponLocationRight.Fire();
    }
}
