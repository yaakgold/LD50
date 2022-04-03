using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    Weapon left, right;

    bool canAttemptFireLeft = true;
    bool canAttemptFireRight = true;

    float currentResetTimeLeft, resetTimeLeft;
    float currentResetTimeRight, resetTimeRight;

    public AttackState(Weapon lft, Weapon rgt, float rstTime)
    {
        left = lft;
        right = rgt;
        currentResetTimeRight = resetTimeRight = currentResetTimeLeft = resetTimeLeft = rstTime;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (left != null)
        {
            if (canAttemptFireLeft && left.CanFire())
            {
                left.Fire();
                GameManager.Instance.StartCoroutine(TimerLeft());
            }
            else if (right != null)
            {
                if (canAttemptFireRight && right.CanFire())
                {
                    right.Fire();
                    GameManager.Instance.StartCoroutine(TimerRight());
                }
            }
        }
        else if (right != null)
        {
            if (canAttemptFireRight && right.CanFire())
            {
                right.Fire();
                GameManager.Instance.StartCoroutine(TimerRight());
            }
        }
    }

    private IEnumerator TimerLeft()
    {
        canAttemptFireLeft = false;

        while (currentResetTimeLeft > 0)
        {
            currentResetTimeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetTimeLeft = Random.Range(resetTimeLeft/ 1.25f, resetTimeLeft * 1.25f);

        currentResetTimeLeft = resetTimeLeft;

        canAttemptFireLeft = true;
    }

    private IEnumerator TimerRight()
    {
        canAttemptFireRight = false;

        while (currentResetTimeRight > 0)
        {
            currentResetTimeRight -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetTimeRight = Random.Range(resetTimeRight / 1.25f, resetTimeRight * 1.25f);

        currentResetTimeRight = resetTimeRight;

        canAttemptFireRight = true;
    }
}
