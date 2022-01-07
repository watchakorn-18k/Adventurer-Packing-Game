using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveName : MonoBehaviour
{
    public static Text obj_txt;
    public InputField obj_input;
    void Start()
    {
        obj_txt = GameObject.Find("Name").GetComponent<Text>();
        obj_txt.text = PlayerPrefs.GetString("SaveName");

    }

    // Update is called once per frame
    void Update()
    {
        obj_txt.text = "สวัสดีคุณ " + obj_input.text;

    }

    public void SaveAndToMenu()
    {
        if (obj_input.text != "")
        {
            Save();
            Application.LoadLevel("Menu");


        }


    }
    void Save()
    {
        PlayerPrefs.SetString("SaveName", obj_input.text);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();

        CreateText();
    }
    void CreateText()
    {
        string path = Application.dataPath + "/SaveName.txt";
        File.AppendAllText(path, obj_txt.text + "\n");

    }
}
