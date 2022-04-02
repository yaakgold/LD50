using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlderSiblingAI : EnemyAI
{
    private StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        //Create states
        GetInRangeState gir = new GetInRangeState(speed, this, GameManager.Instance.player.transform);
        AttackState attack = new AttackState(weaponLocationLeft.GetWeapon(), weaponLocationRight.GetWeapon(), attackSpeed);

        //Create transitions
        At(gir, attack, checkPlayerInRange(), false);
        At(attack, gir, checkPlayerOutOfRange(), false);

        //Create any transitions

        //Create transition helper methods
        void At(IState from, IState to, Func<bool> condition, bool transitionToSelf) => _stateMachine.AddTransition(from, to, condition, transitionToSelf);
        Func<bool> checkPlayerInRange() => () =>
        {
            return Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) < range;
        };

        Func<bool> checkPlayerOutOfRange() => () =>
        {
            return Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) > range;
        };

        //Set default state
        _stateMachine.SetState(gir);
    }

    private void Start()
    {
        if(TryGetComponent(out Health health))
        {
            health.Death.AddListener(OnDeath);
        }
    }

    private void Update()
    {
        base.UpdateCall();

        _stateMachine.Tick();

        transform.position += (Vector3)movement;
    }

    private void OnDeath()
    {
        if(transform.parent.TryGetComponent(out Room room))
        {
            room.RemoveEnemy(gameObject);
        }

        Destroy(gameObject);
    }
}
