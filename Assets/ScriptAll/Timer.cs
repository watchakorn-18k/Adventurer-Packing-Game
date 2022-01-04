using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] int CountDownStart = 120;
    [SerializeField] Text TimerText;
    TimeSpan timeSpan;
    [SerializeField] GameObject gameOver;
    Animator animator;



    Text ScorePackage;

    void Start()
    {
        ScorePackage = GameObject.Find("ScorePackage").GetComponent<Text>();
        animator = GameObject.Find("BgClock").GetComponent<Animator>();


        CountDownTimer();



    }

    void CountDownTimer()
    {

        if (CountDownStart >= 0)
        {
            timeSpan = TimeSpan.FromSeconds(CountDownStart);
            TimerText.text = String.Format(@"{0:mm\:ss}", timeSpan);
            StartCoroutine(WaitAnimationClock());
        }
        else
        {
            ShowGameOver();
        }
    }
    IEnumerator WaitAnimationClock()
    {
        yield return new WaitForSeconds(1.10f);

        CountDownStart--;
        Invoke("CountDownTimer", 1.0f);

    }

    void ShowGameOver()
    {
        Debug.Log("Time is up");
        Time.timeScale = 0f;
        gameOver.SetActive(true);
        Text ScorePackageMenu;
        ScorePackageMenu = GameObject.Find("ScorePackageMenu").GetComponent<Text>();
        ScorePackageMenu.text = ScorePackage.text;

    }

    void Update()
    {

    }
}
