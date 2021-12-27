using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource SoundClickPlay;
    public void PlayGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
