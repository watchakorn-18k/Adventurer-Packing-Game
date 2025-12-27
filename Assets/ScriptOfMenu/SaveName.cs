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
        PlayerPrefs.SetInt("CurrentScore", 0); // [WebGL Fix] Reset current score
        PlayerPrefs.Save();
        // [WebGL Fix] ลบการใช้ File.Create/AppendAllText ออก
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
        // [WebGL Fix] ใช้ PlayerPrefs แทน File
        return PlayerPrefs.GetString("SaveName", "Player");
    }
    string checkScore()
    {
        // [WebGL Fix] ใช้ PlayerPrefs แทน File
        int score = PlayerPrefs.GetInt("CurrentScore", 0);
        return score.ToString();
    }
}
