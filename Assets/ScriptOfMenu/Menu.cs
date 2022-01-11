using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public AudioSource SoundClickPlay;

    public static bool CheckedMusic;
    public static bool CheckedFullScreen;

    public static int CheckedQulity;

    public static bool IsPanelReturn;

    AudioSource MusicSoundTag;

    void Start()
    {
        MusicSoundTag = GameObject.FindWithTag("MusicBackground").GetComponent<AudioSource>();
        CheckedMusic = true;
        CheckedFullScreen = true;
    }

    void Update()
    {
        CheckToggleMusic();
        CheckToggleFullScreen();
        CheckedDropdownQulity();
        CheckIsPanelReturn();

    }

    public void PlayGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void OptionGame()
    {

        StartCoroutine(WaitSoundClickToOptionGame());

    }
    IEnumerator WaitSoundClickToOptionGame()
    {
        SoundClickPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        //do something
    }

    public void HelpConTrol()
    {

        StartCoroutine(WaitSoundClickToHelpConTrol());

    }
    IEnumerator WaitSoundClickToHelpConTrol()
    {
        SoundClickPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
        //do something
    }

    public void PlaySoundClick()
    {
        SoundClickPlay.Play();
    }
    public void QuitGame()
    {
        StartCoroutine(WaitSoundClickToQuit());

    }
    IEnumerator WaitSoundClickToQuit()
    {
        SoundClickPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        Application.Quit();
        //do something
    }

    void CheckToggleMusic()
    {
        try
        {
            if (MenuOption.CheckMusic.isOn == null)
            {

                MusicSoundTag.mute = false;
            }
            else if (MenuOption.CheckMusic.isOn)
            {
                Debug.Log("Music on");
                CheckedMusic = true;
                MusicSoundTag.mute = false;
            }
            else
            {
                Debug.Log("Music off");
                CheckedMusic = false;
                MusicSoundTag.mute = true;
            }
        }
        catch
        {
        }
    }

    void CheckIsPanelReturn()
    {
        try
        {
            if (MenuOption.PanelDisplay.isOn == null)
            {
            }
            else if (MenuOption.PanelDisplay.isOn)
            {
                IsPanelReturn = true;
            }
            else
            {
                IsPanelReturn = false;
            }
        }
        catch
        {
        }
    }


    void CheckToggleFullScreen()
    {
        try
        {
            if (MenuOption.CheckFullScreen.isOn == null)
            {
                CheckedFullScreen = true;
            }
            else if (MenuOption.CheckFullScreen.isOn)
            {
                CheckedFullScreen = true;
            }
            else
            {
                CheckedFullScreen = false;
            }
        }
        catch
        {
            CheckedFullScreen = true;
        }
    }
    void CheckedDropdownQulity()
    {
        try
        {
            CheckedQulity = MenuOption.CheckQulity.value;
        }
        catch
        {
        }
    }
}
