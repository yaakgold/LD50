using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Vector2 movement;
    public WeaponLocation weaponLocationLeft, weaponLocationRight;
    public WeaponRotation weaponRotation;
    [SerializeField] protected float range;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackSpeed;

    protected void UpdateCall()
    {
        weaponRotation.lookLocation = GameManager.Instance.player.transform.position;
    }
}
