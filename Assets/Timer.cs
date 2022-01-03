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


    Text ScorePackage;

    void Start()
    {
        ScorePackage = GameObject.Find("ScorePackage").GetComponent<Text>();

        CountDownTimer();


    }

    void CountDownTimer()
    {

        if (CountDownStart >= 0)
        {
            timeSpan = TimeSpan.FromSeconds(CountDownStart);
            TimerText.text = String.Format(@"{0:mm\:ss}", timeSpan);

            CountDownStart--;
            Invoke("CountDownTimer", 1.0f);
        }
        else
        {
            ShowGameOver();


        }
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
