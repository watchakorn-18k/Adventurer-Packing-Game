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

    // Start is called before the first frame update
    void Start()
    {
        ScorePlayer = int.Parse(checkScore());
        highscore = PlayerPrefs.GetInt("HighScore");
        Debug.Log(highscore);



    }

    // Update is called once per frame
    void Update()
    {
        if (ScorePlayer > highscore)
        {
            highscore = ScorePlayer;
            PlayerPrefs.SetInt("HighScore", highscore);
            PlayerPrefs.SetString("HighScoreNamePlayer", PlayerPrefs.GetString("SaveName").ToString());
            SaveFinalScore();
        }
        obj_txt_score_top.text = highscore.ToString();
        obj_txt_name_top.text = PlayerPrefs.GetString("HighScoreNamePlayer");


    }
    string checkScore()
    {
        string path = "Assets/Resources/SaveScore.txt";
        string[] lines = System.IO.File.ReadAllLines(path);
        string ScoreTxt = lines[lines.Length - 1];
        return ScoreTxt;
    }
    void SaveFinalScore()
    {
        string path = Application.dataPath + "/Resources/SaveFinalScore.txt";
        File.AppendAllText(path, PlayerPrefs.GetInt("HighScore").ToString() + "\n");
    }
}
