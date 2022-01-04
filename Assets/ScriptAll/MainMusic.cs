using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    public AudioSource mainmusic;
    void Start()
    {
        mainmusic.Play();
        try
        {
            if (MenuOption.CheckMusic.isOn == null)
            {
                Debug.Log("do not have everything");
                mainmusic.volume = 0.432f;
            }
            else if (MenuOption.CheckMusic.isOn)
            {
                Debug.Log("Music on");
                mainmusic.volume = 0.432f;
            }
            else
            {
                Debug.Log("Music off");
                mainmusic.volume = 0;
            }
        }
        catch
        {
            mainmusic.volume = 0.432f;
        }
    }

}
