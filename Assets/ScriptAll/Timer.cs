using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public static int CountDownStart = 400;
    [SerializeField] Text TimerText;
    TimeSpan timeSpan;
    [SerializeField] GameObject gameOver;
    Animator animator;

    public Animator Camera_animation;




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
        // Debug.Log(CountDownStart);

    }

    void ShowGameOver()
    {
        StartCoroutine(WaitForAnimation());
        IEnumerator WaitForAnimation()
        {
            Camera_animation.SetBool("IsZoom", false);
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 0f;
            gameOver.SetActive(true);
            Text ScorePackageMenu;
            ScorePackageMenu = GameObject.Find("ScorePackageMenu").GetComponent<Text>();
            ScorePackageMenu.text = ScorePackage.text;
        }
    }

    void Update()
    {

    }
}
