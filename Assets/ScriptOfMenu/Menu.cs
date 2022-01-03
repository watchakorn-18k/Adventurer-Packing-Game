using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public AudioSource SoundClickPlay;

    public static bool CheckedMusic;

    AudioSource MusicSoundTag;

    void Start()
    {
        MusicSoundTag = GameObject.FindWithTag("MusicBackground").GetComponent<AudioSource>();
        CheckedMusic = true;
    }

    void Update()
    {
        try
        {
            if (MenuOption.CheckMusic.isOn == null)
            {
                Debug.Log("do not have everything");
                MusicSoundTag.volume = 1;
            }
            else if (MenuOption.CheckMusic.isOn)
            {
                Debug.Log("Music on");
                CheckedMusic = true;
                MusicSoundTag.volume = 1;
            }
            else
            {
                Debug.Log("Music off");
                CheckedMusic = false;
                MusicSoundTag.volume = 0;
            }
        }
        catch
        {
            MusicSoundTag.volume = 1;
        }
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
}
