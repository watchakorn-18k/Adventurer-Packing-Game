using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] int steerSpeed = 300;
    [SerializeField] int moveSpeed = 20;

    bool isFullPlaceBlue;
    bool isFullPlaceBlack;





    void Update()
    {
        float steerAmout = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        float moveAmout = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmout);
        transform.Translate(0, moveAmout, 0);
        isFullPlaceBlue = Delivery.isFullPlaceBlue;
        isFullPlaceBlack = Delivery.isFullPlaceBlack;

    }


    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "packages")
        {
            moveSpeed = 10;

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlaceBlueFull();
        CheckPlaceBlackFull();
        // if (other.tag == "customerBlack" || other.tag == "customerBlue")
        // {
        //     moveSpeed = 20;
        // }
    }

    void CheckPlaceBlueFull()
    {
        if (isFullPlaceBlue)
        {
            moveSpeed = 10;

        }
        else
        {
            moveSpeed = 20;
        }


    }
    void CheckPlaceBlackFull()
    {
        if (isFullPlaceBlack)
        {
            moveSpeed = 10;

        }
        else
        {
            moveSpeed = 20;
        }


    }
}
