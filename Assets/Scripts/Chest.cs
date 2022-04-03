using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int cost = 10;
    public eRarity rarity = eRarity.NORMAL;

    public GameObject[] normalItems;
    public GameObject[] rareItems;
    public GameObject[] epicItems;

    Vector3 offset = new Vector3(0, 0, -1);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && GameManager.Instance.playerInteraction)
        {
            GameManager.Instance.playerInteraction = false;
            ValidateOpen();
        }
    }

    //Checks that the player has enough money
    public void ValidateOpen()
    {
        if(GameManager.Instance.currentMoneyAmount > cost)
        {
            print("Open");
            GameManager.Instance.currentMoneyAmount -= cost;

            Open();
        }
    }

    private void Open()
    {
        switch (rarity)
        {
            case eRarity.NORMAL:
                Instantiate(normalItems[Random.Range(0, normalItems.Length)], transform.position + offset, Quaternion.identity, transform.parent);
                break;
            case eRarity.RARE:
                Instantiate(rareItems[Random.Range(0, rareItems.Length)], transform.position + offset, Quaternion.identity, transform.parent);
                break;
            case eRarity.EPIC:
                Instantiate(epicItems[Random.Range(0, epicItems.Length)], transform.position + offset, Quaternion.identity, transform.parent);
                break;
        }
        Destroy(gameObject);

        GameManager.Instance.SpendMoney(cost);
    }
}

public enum eRarity
{
    NORMAL,
    RARE,
    EPIC
}