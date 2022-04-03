using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUPDeage : Powerup
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.playerInteraction)
        {
            GameManager.Instance.playerInteraction = false;

            action = () => { GameManager.Instance.AgePlayer(-(int)amount); return true; };

            GameManager.Instance.GainPowerup(this);
            Destroy(gameObject);
        }
    }
}
