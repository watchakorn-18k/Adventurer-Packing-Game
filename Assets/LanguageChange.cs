using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageChange : MonoBehaviour
{
    public static string Language;
    // Start is called before the first frame update
    void Start()
    {
        CheckLanguage();


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CheckLanguage()
    {
        if (MainMenu.Language == "Thai")
        {
            Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Thai");
        }
        else if (MainMenu.Language == "English")
        {
            Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
        }
    }
}
