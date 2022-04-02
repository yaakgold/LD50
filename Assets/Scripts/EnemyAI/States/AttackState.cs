using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    Weapon left, right;

    bool canAttemptFireLeft = true;
    bool canAttemptFireRight = true;

    float currentResetTime, resetTime;

    public AttackState(Weapon lft, Weapon rgt, float rstTime)
    {
        left = lft;
        right = rgt;
        currentResetTime = resetTime = rstTime;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Attack state");
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
                GameManager.Instance.StartCoroutine(Timer(true));
            }
            else if (right != null)
            {
                if (canAttemptFireRight && right.CanFire())
                {
                    right.Fire();
                    GameManager.Instance.StartCoroutine(Timer(false));
                }
            }
        }
        else if (right != null)
        {
            if (canAttemptFireRight && right.CanFire())
            {
                right.Fire();
                GameManager.Instance.StartCoroutine(Timer(false));
            }
        }
    }

    private IEnumerator Timer(bool left)
    {
        if(left) canAttemptFireLeft = false;
        else canAttemptFireRight = false;

        while (currentResetTime > 0)
        {
            currentResetTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetTime = Random.Range(resetTime / 1.25f, resetTime * 1.25f);

        currentResetTime = resetTime;

        if (left) canAttemptFireLeft = true;
        else canAttemptFireRight = true;
    }
}
