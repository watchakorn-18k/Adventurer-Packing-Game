using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceCustomBlack : MonoBehaviour
{

    TextMesh TxtReq;
    GameObject CustomPlace;
    void Start()
    {
        TxtReq = GameObject.Find("FloatingTextBlack").GetComponent<TextMesh>();
        CustomPlace = GameObject.Find("CustomerBlack");
    }

    // Update is called once per frame
    void Update()
    {

        TxtReq.transform.position = new Vector3(CustomPlace.transform.position.x, CustomPlace.transform.position.y - 1.917f, -0.5f);
    }
}
