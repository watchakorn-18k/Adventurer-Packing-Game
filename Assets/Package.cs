using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    BoxCollider2D boxCollider;
    GameObject Car;

    void Update()
    {
        //เรียกใช้ gameObject ที่มีชื่อ Tooy Mcdrive เพื่อยกของขึ้นรถ
        Car = GameObject.Find("Tooy Mcdrive");

        //ทำลาย BoxCollider2D
        Destroy(gameObject.GetComponent<BoxCollider2D>());

        //ย้ายตำแหน่งไปยังตำแหน่งของรถ
        transform.position = Car.transform.position + new Vector3(0, -0.2f, -5);
        // transform.localScale = new Vector3(0.5220157f, 0.5468475f, 0.5453f);
    }
}
