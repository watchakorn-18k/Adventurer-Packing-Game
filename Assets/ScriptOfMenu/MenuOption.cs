using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOption : MonoBehaviour
{
    public AudioSource SoundClickPlay;
    public static Toggle CheckMusic;

    public static Toggle CheckFullScreen;

    public static Dropdown CheckQulity;

    public static bool IsPanel;

    public static Toggle PanelDisplay;




    AudioSource MusicSoundTag;



    public void Start()
    {
        CheckLanguage();

        MusicSoundTag = GameObject.FindWithTag("MusicBackground").GetComponent<AudioSource>();
        CheckMusic = GameObject.Find("CheckMusic").GetComponent<Toggle>();
        CheckFullScreen = GameObject.Find("FullSceeneToggle").GetComponent<Toggle>();
        PanelDisplay = GameObject.Find("CheckPanelKeyborad").GetComponent<Toggle>();
        CheckQulity = GameObject.Find("GraphicDropdown").GetComponent<Dropdown>();
        CheckBoolMusicFromMenu();
        CheckBoolFullSreenFromMenu();
        CheckBoolCheckedQulityFromMenu();
        CheckPannelPublicFromMenu();
        CheckPanelKeyboardOnOff();



    }

    public void Update()
    {
        CheckMuteMusic();




    }

    public void CheckLanguage()
    {
        if (MainMenu.Language == "Thai")
        {
            Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Thai");
        }
        else if (MainMenu.Language == "English")
        {
            Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
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

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log(Screen.fullScreen);
    }

    void CheckMuteMusic()
    {
        if (!CheckMusic.isOn)
        {
            MusicSoundTag.volume = 0;

        }
        else
        {
            MusicSoundTag.volume = 1;

        }

    }

    void CheckBoolMusicFromMenu()
    {
        if (!Menu.CheckedMusic)
        {
            CheckMusic.isOn = false;
        }
        else
        {
            CheckMusic.isOn = true;
        }
    }

    void CheckBoolFullSreenFromMenu()
    {
        if (!Menu.CheckedFullScreen)
        {
            CheckFullScreen.isOn = false;
        }
        else
        {
            CheckFullScreen.isOn = true;

        }
    }

    void CheckBoolCheckedQulityFromMenu()
    {
        CheckQulity.value = Menu.CheckedQulity;
    }

    void CheckPanelKeyboardOnOff()
    {
        if (PanelDisplay.isOn)
        {
            IsPanel = true;
        }
        else
        {
            IsPanel = false;
        }


    }

    void CheckPannelPublicFromMenu()
    {
        if (Menu.IsPanelReturn) PanelDisplay.isOn = true;
        else PanelDisplay.isOn = false;
    }
}
