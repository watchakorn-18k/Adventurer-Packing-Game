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
        Debug.Log(PlayerPrefs.GetString("SaveName"));
        Debug.Log(PlayerPrefs.GetInt("Score"));
        obj_txt_name_top.text = PlayerPrefs.GetString("SaveName");
        obj_txt_score_top.text = PlayerPrefs.GetInt("Score").ToString();

    }
    public void ChangeName()
    {
        Application.LoadLevel("Name");
    }
}
