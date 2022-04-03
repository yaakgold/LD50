using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInRangeState : IState
{
    float speed;
    EnemyAI controller;
    Transform target;

    public GetInRangeState(float spd, EnemyAI ai, Transform trg)
    {
        speed = spd;
        controller = ai;
        target = trg;
    }

    public void OnEnter()
    {
        Debug.LogError("Enter GIR state");
    }

    public void OnExit()
    {
        controller.movement *= 0;
    }

    public void Tick()
    {
        controller.movement = (target.position - controller.transform.position).normalized * speed * Time.deltaTime;
    }
}
