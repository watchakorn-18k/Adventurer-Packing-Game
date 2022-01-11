using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUIinGame : MonoBehaviour
{
    // Start is called before the first frame update
    bool IsPanel;
    void Start()
    {
        IsPanel = MenuOption.IsPanel;
        if (IsPanel == true)
        {
            GameObject.Find("ControlDisplay").SetActive(true);
        }
        else
        {
            GameObject.Find("ControlDisplay").SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
