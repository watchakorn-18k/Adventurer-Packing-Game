using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowNameMenue : MonoBehaviour
{
    public Text obj_txt_name_top;
    public Text obj_txt_score_top;
    void Start()
    {



    }

    void Update()
    {
        obj_txt_name_top.text = PlayerPrefs.GetString("SaveName");
        obj_txt_score_top.text = checkScore().ToString();

    }
    public void ChangeName()
    {
        Application.LoadLevel("Name");
    }
    string checkScore()
    {
        // [WebGL Fix] ใช้ PlayerPrefs แทน File
        int score = PlayerPrefs.GetInt("CurrentScore", 0);
        return score.ToString();
    }
}
