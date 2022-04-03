using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static string ScoreString = "LD50Score";

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

    private List<Powerup> powerups = new List<Powerup>();
    public bool ShowHealth { get; private set; }

    public int restarts = 0;

    public float GetAgePercentage()
    {
        return ((float)playerAge / deathAge);
    }

    public void RestartGame()
    {
        restarts++;
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

        if(amount > 0)
        {
            AudioManager.instance.Play("AgeUp");
        }

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
        AudioManager.instance?.Play("SpendMoney");

        currentMoneyAmount -= amt;

        gameUI.UpdateMoneyText();
    }

    public void GainPowerup(Powerup pup)
    {
        AudioManager.instance?.Play("GetPowerup");

        if(pup.timer == 0)
        {
            pup.action();
            return;
        }

        powerups.Add(pup);

        gameUI.AddPup(pup);
    }

    public void RemovePUP(string pName)
    {
        Powerup p = null;

        for (int i = 0; i < powerups.Count; i++)
        {
            if(powerups[i].pupName.ToLower().Trim() == pName.ToLower().Trim())
            {
                p = powerups[i];
                break;
            }
        }

        if(p.pupName != null)
        {
            AudioManager.instance?.Play("LostPowerup");
            p.deactivate();
            powerups.Remove(p);
        }
    }

    public void ToggleShowHealth(bool val)
    {
        ShowHealth = val;
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