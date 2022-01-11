using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerPrefs.GetString("SaveName") == "")
            {
                Application.LoadLevel("Name");
            }
            else
            {
                Application.LoadLevel("Menu");
            }

        }
        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);

        if (PlayerPrefs.GetString("SaveName") == "")
        {
            Application.LoadLevel("Name");
        }
        else
        {
            Application.LoadLevel("Menu");
        }
    }
}
