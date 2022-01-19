using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    public AudioSource SoundPlay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        {
            SoundPlay.Play();
            Application.LoadLevel("SplashScene");

        }
        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        Application.LoadLevel("SplashScene");
    }
    
    public void PlaySoundClick()
    {
        SoundPlay.Play();
    }
}
