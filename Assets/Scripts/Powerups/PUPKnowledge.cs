using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUPKnowledge : Powerup
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.playerInteraction)
        {
            GameManager.Instance.playerInteraction = false;

            action = () =>
            {
                GameManager.Instance.ToggleShowHealth(true);
                return true;
            };

            deactivate = () =>
            {
                GameManager.Instance.ToggleShowHealth(false);
                return true;
            };

            GameManager.Instance.GainPowerup(this);
            Destroy(gameObject);
        }
    }
}
