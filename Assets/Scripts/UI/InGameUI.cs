using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public TMP_Text ageText;
    public TMP_Text hpText;
    public TMP_Text moneyText;
    public GameObject topPanel;
    public GameObject deathScreen;

    public void UpdateAgeText(int age)
    {
        ageText.text = $"Age: {age}";
    }

    public void UpdateHealthText(int health)
    {
        hpText.text = $"HP: {health}";
    }

    public void UpdateMoneyText()
    {
        moneyText.text = $"Money: ${GameManager.Instance.currentMoneyAmount}";
    }

    public void PlayerDieUI()
    {
        topPanel.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void OnRestart(int age)
    {
        GameManager.Instance.playerAge = age - GameManager.Instance.playerAgeAddAmount;

        topPanel.SetActive(true);
        deathScreen.SetActive(false);

        GameManager.Instance.RestartGame();
    }
}
