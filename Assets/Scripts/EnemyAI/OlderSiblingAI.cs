using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OlderSiblingAI : EnemyAI
{
    private StateMachine _stateMachine;

    public bool hasRangedWeapon = false;
    public bool leaveRangeState = false;

    public GameObject moneyObj;
    public int numMoneyDrop = 1;

    Vector3 offset = new Vector3(.25f, .25f, 0);

    [SerializeField] TMP_Text healthBarText;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        //Create states
        GetInRangeState gir = new GetInRangeState(speed, this, GameManager.Instance.player.transform);
        AttackState attack = new AttackState(weaponLocationLeft.GetWeapon(), weaponLocationRight.GetWeapon(), attackSpeed);
        AttackRangedState attackRanged = new AttackRangedState(weaponLocationLeft.GetWeapon(), weaponLocationRight.GetWeapon(), attackSpeed, this);

        //Create transitions
        At(gir, attack, checkPlayerInRange(), false);
        At(attack, gir, checkPlayerOutOfRange(), false);
        At(attackRanged, gir, checkPlayerOutOfRange(), false);
        At(attackRanged, attack, checkPlayerInRange(), false);

        //Create any transitions
        _stateMachine.AddAnyTransition(attackRanged, checkIsInRangeForRangedAttack(), false);

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

        Func<bool> checkIsInRangeForRangedAttack() => () =>
        {
            return hasRangedWeapon && Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) < rangedRange;
        };

        //Set default state
        _stateMachine.SetState(gir);
    }

    private void Start()
    {
        if(TryGetComponent(out Health health))
        {
            health.Death.AddListener(OnDeath);
            health.TookDamage.AddListener(() => AudioManager.instance?.Play("EnemyHurt"));
        }

        numMoneyDrop = UnityEngine.Random.Range(1, 6);
    }

    private void Update()
    {
        base.UpdateCall();

        _stateMachine.Tick();

        transform.position += (Vector3)movement;

        if (movement.magnitude > 0) GetComponentInChildren<Wobble>().doTheWobble = true;
        else GetComponentInChildren<Wobble>().doTheWobble = false;

        healthBarText.gameObject.SetActive(GameManager.Instance.ShowHealth);

        if (TryGetComponent(out Health health))
        {
            healthBarText.text = $"HP: {health.currentHealth}";
        }
    }

    private void OnDeath()
    {
        AudioManager.instance?.Play("EnemyDie");

        if (transform.parent.TryGetComponent(out Room room))
        {
            room.RemoveEnemy(gameObject);
        }

        for (int i = 0; i < numMoneyDrop; i++)
        {
            Instantiate(moneyObj, transform.position + (offset * i), Quaternion.identity, transform.parent);
        }

        Destroy(gameObject);
    }
}
