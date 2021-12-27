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
    public void QuitGame()
    {
        SoundClickPlay.Play();
        Application.Quit();
    }

}
