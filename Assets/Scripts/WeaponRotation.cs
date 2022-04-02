using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] float maxDist;
    [SerializeField] float rotSpeed;

    [SerializeField] WeaponLocation left, right;

    public bool IsNotFiring { get { return left.IsNotFiring && right.IsNotFiring; } }

    public Vector3 lookLocation;
    Vector3 difference;

    // Update is called once per frame
    void Update()
    {
        difference = lookLocation - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (rotation_z + maxDist - 90)), Time.deltaTime * rotSpeed);
    }
}
