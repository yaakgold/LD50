using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PUPDisplay : MonoBehaviour
{
    [SerializeField] Slider timerSlider;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image spriteImage;
    [SerializeField] Image sliderForeground;

    string pupName;
    Sprite spr;
    float timerStart;
    float val;

    InGameUI gameUI;

    public string GetName() { return pupName; }

    public void StartDisplay(string nme, Sprite _spr, float time, InGameUI gmUI)
    {
        pupName = nme;
        spr = _spr;
        val = timerStart = time;

        nameText.text = pupName;
        spriteImage.sprite = spr;

        gameUI = gmUI;

        gameUI.RestartGameEvent.AddListener(RemoveDisplay);

        StartCoroutine(RunUpdate());
    }

    public IEnumerator RunUpdate()
    {
        val = timerStart;

        while(val > 0)
        {
            val -= Time.deltaTime;
            UpdateDisplay();
            yield return new WaitForEndOfFrame();
        }

        RemoveDisplay();
    }

    public void UpdateDisplay()
    {
        timerSlider.value = val / timerStart;
        sliderForeground.color = Color.Lerp(Color.red, Color.green, timerSlider.value);
    }

    public void RemoveDisplay()
    {
        //I may add some extra stuff to the remove, like audio or something so I made an extra step just in case

        gameUI.RemovePup(this);

        Destroy(gameObject);
    }

    public void ResetTimer(float time)
    {
        timerStart = val = time;

        UpdateDisplay();
    }
}
