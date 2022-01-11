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
        string pathScore = Application.dataPath + "/Resources/SaveScore.txt";
        string pathName = Application.dataPath + "/Resources/SaveName.txt";
        File.Create(pathScore).Close();
        File.Create(pathName).Close();
        File.AppendAllText(pathScore, 0 + "\n");

    }


    // string ShowNameFinalScore()
    // {
    //     string path = "Assets/Resources/SaveFinalScore.txt";
    //     string[] lines = System.IO.File.ReadAllLines(path);
    //     List<string> listscore = new List<string>();
    //     int ScoreNumber = 0;

    //     foreach (string c in lines)
    //     {
    //         // Debug.Log(c[c.Length - 1]);
    //         listscore.Add(c[c.Length - 1].ToString());
    //     }
    //     listscore.Sort();
    //     Debug.Log(listscore[0]);

    //     string NamePlayerRank = listscore[1].ToString();
    //     return NamePlayerRank;
    // }

    string checkName()
    {
        string path = Application.dataPath + "/Resources/SaveName.txt";
        string[] lines = System.IO.File.ReadAllLines(path);
        string NameTxt = lines[lines.Length - 1];
        return NameTxt;
    }
    string checkScore()
    {
        string path = Application.dataPath + "/Resources/SaveScore.txt";
        string[] lines = System.IO.File.ReadAllLines(path);
        string ScoreTxt = lines[lines.Length - 1];
        return ScoreTxt;
    }
}
