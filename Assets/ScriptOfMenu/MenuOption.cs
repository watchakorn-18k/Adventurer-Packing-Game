using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOption : MonoBehaviour
{
    public AudioSource SoundClickPlay;
    public static Toggle CheckMusic;

    public GameObject OptionMenu;

    AudioSource MusicSoundTag;



    public void Start()
    {

        OptionMenu = GameObject.Find("Option");
        MusicSoundTag = GameObject.FindWithTag("MusicBackground").GetComponent<AudioSource>();
        CheckMusic = GameObject.Find("CheckMusic").GetComponent<Toggle>();
        if (!Menu.CheckedMusic)
        {
            CheckMusic.isOn = false;
        }
        else
        {
            CheckMusic.isOn = true;
        }
    }

    public void Update()
    {
        Debug.Log(CheckMusic.isOn);
        if (!CheckMusic.isOn)
        {
            MusicSoundTag.volume = 0;

        }
        else
        {
            MusicSoundTag.volume = 1;


        }

    }

    public void PlaySoundClick()
    {
        SoundClickPlay.Play();
    }
    public void BackToMenu()
    {
        StartCoroutine(WaitSoundClickToBackToMenu());

    }
    IEnumerator WaitSoundClickToBackToMenu()
    {
        SoundClickPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
        //do something
    }
}
