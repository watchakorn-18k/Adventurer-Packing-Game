using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour

{
    public AudioSource SoundClickPlay;

    public void Restart()
    {
        StartCoroutine(WaitSoundClickToRestart());
    }
    IEnumerator WaitSoundClickToRestart()
    {
        SoundClickPlay.Play();
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.4179592f);
        Application.LoadLevel(Application.loadedLevel);

        //do something
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SoundClickPlay.Play();
        Delivery.isFullPlaceBlue = false;
        Delivery.isFullPlaceBlack = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
    public void QuitGame()
    {
        SoundClickPlay.Play();
        Application.Quit();
    }

}
