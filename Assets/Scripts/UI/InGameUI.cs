using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public TMP_Text ageText;
    public TMP_Text hpText;
    public TMP_Text moneyText;
    public GameObject topPanel;
    public GameObject deathScreen;
    public GameObject pupList;
    public GameObject pupDisplayObj;
    public Button btn5YO;
    public Button btnHalfYO;
    public TMP_Text endScreenText;

    [HideInInspector]
    public UnityEvent RestartGameEvent;

    List<PUPDisplay> pupDisplays;

    private void Start()
    {
        pupDisplays = new List<PUPDisplay>();

        btn5YO.onClick.AddListener(() => OnRestart(5, 1000));
    }

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
        pupList.SetActive(false);

        if(GameManager.Instance.playerAge <= GameManager.Instance.deathAge)
        {
            endScreenText.text =
                $"You were killed on your pursuit of a full life.\n\nYou died at <color=blue>{GameManager.Instance.playerAge} years old</color> with <color=green>${GameManager.Instance.currentMoneyAmount}</color>\n\nYou have a chance to continue your life. Here are your options";

            int age = (int)((GameManager.Instance.playerAge - 5) * .5f);
            btnHalfYO.GetComponentInChildren<TMP_Text>().text = $"Restart at {age} years old, for a cost of ${500}";
            btnHalfYO.onClick.AddListener(() => OnRestart(age, 500));
        }
        else
        {
            endScreenText.text = $"You finished your pursuit of a full life.\n\nYou died at <color=blue>{GameManager.Instance.deathAge}</color> after {GameManager.Instance.restarts + 1} lives.\n\nAfter everything is said and done, you made <color=green>${GameManager.Instance.currentMoneyAmount}!";
            btn5YO.gameObject.SetActive(false);
            btnHalfYO.gameObject.SetActive(false);
        }

        deathScreen.SetActive(true);
    }

    public void OnRestart(int age, int cost)
    {
        AudioManager.instance?.Play("BtnClick");

        if (cost > GameManager.Instance.currentMoneyAmount) return;

        GameManager.Instance.SpendMoney(cost);
        GameManager.Instance.playerAge = (age - GameManager.Instance.playerAgeAddAmount);

        topPanel.SetActive(true);
        pupList.SetActive(true);
        deathScreen.SetActive(false);

        RestartGameEvent.Invoke();

        RestartGameEvent.RemoveAllListeners();

        GameManager.Instance.RestartGame();
    }

    public void EndGameFully()
    {
        AudioManager.instance?.Play("BtnClick");

        int highScore = PlayerPrefs.GetInt(GameManager.ScoreString, 0);

        if(GameManager.Instance.currentMoneyAmount > highScore)
        {
            PlayerPrefs.SetInt(GameManager.ScoreString, GameManager.Instance.currentMoneyAmount);
        }

        SceneManager.LoadScene(0);
    }

    public PUPDisplay AddPup(Powerup p)
    {
        PUPDisplay disp = FindPUPInList(p);

        if (disp != null)
        {
            disp.ResetTimer(p.timer);

            return disp;
        }

        p.action();

        disp = Instantiate(pupDisplayObj, pupList.transform).GetComponent<PUPDisplay>();

        disp.StartDisplay(p.pupName, p.spr, p.timer, this);

        pupDisplays.Add(disp);

        return disp;
    }

    public void RemovePup(PUPDisplay disp)
    {
        GameManager.Instance.RemovePUP(disp.GetName());
        pupDisplays.Remove(disp);
    }

    private PUPDisplay FindPUPInList(Powerup p)
    {
        return pupDisplays.SingleOrDefault(x => x.GetName() == p.pupName);
    }
}
