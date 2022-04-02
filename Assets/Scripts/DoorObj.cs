using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObj : MonoBehaviour
{
    public Room owner;
    public door door;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(door.doorVal == eDoorVal.UNLOCKED && GameManager.Instance.playerInteraction)
        {
            owner.ExitRoom();

            if(door.neighbor.TryGetComponent(out Room neighRoom))
            {
                GameManager.Instance.playerInteraction = false;
                neighRoom.EnterRoom();
            }
        }
    }
}
