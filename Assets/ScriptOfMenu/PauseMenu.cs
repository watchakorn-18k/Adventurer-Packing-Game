using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public AudioSource SoundClickPlay;
    public Animator Camera_animation;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (GameIsPaused)
            {
                Camera_animation.SetBool("IsZoom", true);
                Resume();
            }
            else
            {
                Pause();
            }
        }


    }
    public void SoundClick()
    {
        SoundClickPlay.Play();
    }

    public void Resume()
    {

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;


    }


    void Pause()
    {

        StartCoroutine(WaitForAnimation());
        IEnumerator WaitForAnimation()
        {
            Camera_animation.SetBool("IsZoom", false);
            yield return new WaitForSeconds(0.3f);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

    }

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
        StartCoroutine(WaitSoundClickToLoadMenu());
    }
    IEnumerator WaitSoundClickToLoadMenu()
    {
        SoundClickPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        Delivery.isFullPlaceBlue = false;
        Delivery.isFullPlaceBlack = false;
        SceneManager.LoadScene("Menu");

        //do something
    }


    public void QuitGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitSoundClickToQuit());
    }
    IEnumerator WaitSoundClickToQuit()
    {
        SoundClickPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        Application.Quit();

        //do something
    }

}
