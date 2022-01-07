using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNamePlayer : MonoBehaviour
{
    Text NameTextInGame;

    // Update is called once per frame
    void Update()
    {
        NameTextInGame = GameObject.Find("NameTextInGame").GetComponent<Text>();
        NameTextInGame.text = PlayerPrefs.GetString("SaveName");

    }
}
