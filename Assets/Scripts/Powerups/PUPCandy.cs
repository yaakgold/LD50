using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUPCandy : Powerup
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.playerInteraction)
        {
            GameManager.Instance.playerInteraction = false;

            action = () => 
            {
                if (GameManager.Instance.player.TryGetComponent(out PlayerController pc))
                {
                    pc.SetSpeed(pc.defaultSpeed * amount);
                }
                return true; 
            };

            deactivate = () =>
            {
                if (GameManager.Instance.player == null) return false;

                if (GameManager.Instance.player.TryGetComponent(out PlayerController pc))
                {
                    pc.SetSpeed(pc.defaultSpeed);
                }
                return true;
            };

            GameManager.Instance.GainPowerup(this);
            Destroy(gameObject);
        }
    }
}
