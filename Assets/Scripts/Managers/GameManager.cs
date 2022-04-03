using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public const int studentAge = 15;
    public const int adultAge = 30;
    public const int oldAge = 60;

    public int playerAge = 5;
    public int deathAge = 70;
    public int playerAgeAddAmount = 5;
    public int amountToHealOnRoomEnter = 20;
    public eAgeGroup currentAgeGroup = eAgeGroup.YOUNG;

    public GameObject player;
    public bool playerInteraction = false;
    public InGameUI gameUI;

    [SerializeField] GameObject playerPref;

    public Sprite[] playerSprites;

    public AgedWeapons meleeWeaponY, rangedWeaponY;
    public AgedWeapons meleeWeaponS, rangedWeaponS;
    public AgedWeapons meleeWeaponA, rangedWeaponA;
    public AgedWeapons meleeWeaponO, rangedWeaponO;

    public GameObject[] youngEnemies;
    public GameObject[] studentEnemies;
    public GameObject[] adultEnemies;
    public GameObject[] oldEnemies;

    public Room currentRoom;

    public int currentMoneyAmount;
    [SerializeField] int moneyWorth;

    public float GetAgePercentage()
    {
        return ((float)playerAge / deathAge);
    }

    public void RestartGame()
    {
        RoomManager.Instance.SetupGameMap();
    }

    public void StartGame()
    {
        player = Instantiate(playerPref, new Vector3(0, 0, -1), Quaternion.identity);

        if(player.TryGetComponent(out Health health))
        {
            gameUI.UpdateHealthText(health.currentHealth);
        }

        RoomManager.Instance.GetRooms()[0].EnterRoom();
    }

    public void EndGame()
    {
        Destroy(player);
        player = null;

        int childCount = RoomManager.Instance.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(RoomManager.Instance.transform.GetChild(i).gameObject);
        }

        gameUI.PlayerDieUI();
    }

    public void AgePlayer(int amount = 0)
    {
        if (amount == 0) amount = playerAgeAddAmount;

        playerAge = Mathf.Clamp(playerAge + amount, 5, deathAge + 10);

        gameUI.UpdateAgeText(playerAge);

        if(playerAge >= oldAge)
        {
            if(player.TryGetComponent(out PlayerController pc))
            {
                currentAgeGroup = eAgeGroup.OLD;
                pc.UpdateAge(playerSprites[3]);
                pc.SetLeftWeapon(meleeWeaponO.weaponPref);
                pc.SetRightWeapon(rangedWeaponO.weaponPref);
            }
        }
        else if (playerAge >= adultAge)
        {
            if (player.TryGetComponent(out PlayerController pc))
            {
                currentAgeGroup = eAgeGroup.ADULT;
                pc.UpdateAge(playerSprites[2]);
                pc.SetLeftWeapon(meleeWeaponA.weaponPref);
                pc.SetRightWeapon(rangedWeaponA.weaponPref);
            }
        }
        else if (playerAge >= studentAge)
        {
            if (player.TryGetComponent(out PlayerController pc))
            {
                currentAgeGroup = eAgeGroup.STUDENT;
                pc.UpdateAge(playerSprites[1]);
                pc.SetLeftWeapon(meleeWeaponS.weaponPref);
                pc.SetRightWeapon(rangedWeaponS.weaponPref);
            }
        }
        else
        {
            if (player.TryGetComponent(out PlayerController pc))
            {
                currentAgeGroup = eAgeGroup.YOUNG;
                pc.UpdateAge(playerSprites[0]);
                pc.SetLeftWeapon(meleeWeaponY.weaponPref);
                pc.SetRightWeapon(rangedWeaponY.weaponPref);
            }
        }

        if (playerAge > deathAge)
        {
            if(player.TryGetComponent(out Health health))
            {
                health.DoDamage(health.MaxHealth * 2);
            }
        }
    }

    public void AddMoney()
    {
        currentMoneyAmount += moneyWorth;

        gameUI.UpdateMoneyText();
    }

    public void SpendMoney(int amt)
    {
        currentMoneyAmount -= amt;

        gameUI.UpdateMoneyText();
    }

    public void GainPowerup(Powerup pup)
    {
        pup.action();
    }
}

public enum eAgeGroup
{
    YOUNG,
    STUDENT,
    ADULT,
    OLD
}

[System.Serializable]
public class AgedWeapons
{
    public eAgeGroup ageGroup;
    public GameObject weaponPref;
}