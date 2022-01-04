using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
    public GameObject BoxStart;
    AudioSource SoundClickPlay;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        BoxStart = GameObject.Find("StartLevel");
        SoundClickPlay = GameObject.Find("SoundClickPlay").GetComponent<AudioSource>();


    }

    public void GotIt()
    {
        Time.timeScale = 1f;
        BoxStart.SetActive(false);

    }

    public void SoundClick()
    {
        SoundClickPlay.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
