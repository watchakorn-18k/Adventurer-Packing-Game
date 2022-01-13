using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static string Language;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeToThai()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Thai");
        Language = "Thai";
    }

    public void ChangeToEnglish()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
        Language = "English";
    }

}
