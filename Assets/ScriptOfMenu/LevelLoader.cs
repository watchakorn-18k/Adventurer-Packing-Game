using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    AudioSource MusicSoundTag;
    void Start()
    {
        MusicSoundTag = GameObject.FindWithTag("MusicBackground").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        slider.value += 0.0009f;
        if (slider.value == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        try
        {
            if (MenuOption.CheckMusic.isOn == null)
            {

                MusicSoundTag.volume = 0.413f;
            }
            else if (MenuOption.CheckMusic.isOn)
            {

                MusicSoundTag.volume = 0.413f;
            }
            else
            {
                MusicSoundTag.volume = 0;
            }
        }
        catch
        {
            MusicSoundTag.volume = 0.413f;
        }

    }
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(op.progress);
            slider.value = progress;

            yield return null;
        }
    }
}
