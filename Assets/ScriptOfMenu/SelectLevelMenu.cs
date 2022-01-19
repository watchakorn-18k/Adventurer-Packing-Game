using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelMenu : MonoBehaviour
{
    AudioSource SoundPlay;
    AudioSource MusicSoundTag;


    void Start()
    {
        SoundPlay = GameObject.Find("SoundClickPlay").GetComponent<AudioSource>();
        MusicSoundTag = GameObject.FindWithTag("MusicBackground").GetComponent<AudioSource>();


    }
    public void LoadLevel_1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void LoadLevel_2()
    {
        SceneManager.LoadScene("level_2");
    }

    public void BackToMenu()
    {
        StartCoroutine(WaitSoundClickToBackToMenu());

    }
    IEnumerator WaitSoundClickToBackToMenu()
    {
        SoundPlay.Play();
        yield return new WaitForSeconds(0.4179592f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
        //do something
    }

    public void PlaySoundClick()
    {
        SoundPlay.Play();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (MenuOption.CheckMusic.isOn == null)
            {
                Debug.Log("do not have everything");
                MusicSoundTag.volume = 0.413f;
            }
            else if (MenuOption.CheckMusic.isOn)
            {
                Debug.Log("Music on");
                MusicSoundTag.volume = 0.413f;
            }
            else
            {
                Debug.Log("Music off");
                MusicSoundTag.volume = 0;
            }
        }
        catch
        {
            MusicSoundTag.volume = 0.413f;
        }

    }
}
