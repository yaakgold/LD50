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

    public int playerAge = 5;
    public int deathAge = 70;
    public int playerAgeAddAmount = 5;
    public int amountToHealOnRoomEnter = 20;

    public GameObject player;
    public bool playerInteraction = false;
    public InGameUI gameUI;

    [SerializeField] GameObject playerPref;

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
        player = Instantiate(playerPref, Vector3.zero, Quaternion.identity);

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

    public void AgePlayer()
    {
        playerAge += playerAgeAddAmount;

        gameUI.UpdateAgeText(playerAge);

        if(playerAge > deathAge)
        {
            if(player.TryGetComponent(out Health health))
            {
                health.DoDamage(health.MaxHealth * 2);
            }
        }
    }
}

public enum eAgeGroup
{
    YOUNG,
    STUDENT,
    ADULT,
    OLD
}