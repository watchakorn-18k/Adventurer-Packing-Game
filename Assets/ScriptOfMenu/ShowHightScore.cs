using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShowHightScore : MonoBehaviour
{
    int ScorePlayer;
    int highscore;
    public Text obj_txt_score_top;
    public Text obj_txt_name_top;
    public GameObject obj_img_box;

    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs.DeleteAll();
        ScorePlayer = int.Parse(checkScore());
        highscore = PlayerPrefs.GetInt("HighScore");




    }

    // Update is called once per frame
    void Update()
    {
        NewHightScore();
        obj_txt_score_top.text = highscore.ToString();
        obj_txt_name_top.text = PlayerPrefs.GetString("HighScoreNamePlayer");
        if (highscore == 0)
        {
            obj_img_box.SetActive(false);

        }
        else
        {
            obj_img_box.SetActive(true);

        }


    }
    string checkScore()
    {
        // [WebGL Fix] ใช้ PlayerPrefs แทน File
        int score = PlayerPrefs.GetInt("CurrentScore", 0);
        return score.ToString();
    }

    void NewHightScore()
    {
        if (ScorePlayer > highscore)
        {
            highscore = ScorePlayer;
            PlayerPrefs.SetInt("HighScore", highscore);
            PlayerPrefs.SetString("HighScoreNamePlayer", PlayerPrefs.GetString("SaveName").ToString());
            SaveFinalScore();
        }
    }

    void SaveFinalScore()
    {
        // [WebGL Fix] ใช้ PlayerPrefs แทน File I/O
        // HighScore ถูก save ใน NewHightScore() แล้ว
        PlayerPrefs.Save();
    }
}
