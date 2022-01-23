using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour

{
    public AudioSource SoundClickPlay;

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SoundClickPlay.Play();
        Delivery.isFullPlaceBlue = false;
        Delivery.isFullPlaceBlack = false;
        SceneManager.LoadScene("Menu");

    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level_2");

    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SoundClickPlay.Play();
        Application.Quit();
    }

}
