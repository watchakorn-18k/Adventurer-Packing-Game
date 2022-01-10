using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] int steerSpeed = 300;
    [SerializeField] int moveSpeed = 10;

    public GameObject EffectWalk_1;
    public GameObject EffectWalk_2;

    public AudioSource SoundWalk;

    public AudioSource SoundSteer;

    public GameObject EffectWalk;

    Animator animator;

    Rigidbody2D rb;

    bool isFullPlaceBlue;
    bool isFullPlaceBlack;

    bool IsMoving;

    bool IsSpeedUp;
    bool IsSteering;

    float steerAmout;
    float moveAmout;
    float SpeedUp;




    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {


        steerAmout = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        moveAmout = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        SpeedUp = Input.GetAxis("Jump") * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmout);
        transform.Translate(0, moveAmout, 0);
        CheckHoldSpace();
        CheckSoundMove();
        CheckSoundSteer();
        isFullPlaceBlue = Delivery.isFullPlaceBlue;
        isFullPlaceBlack = Delivery.isFullPlaceBlack;
        CheckMove();



    }

    void CheckHoldSpace()
    {
        if (SpeedUp > 0)
        {
            moveSpeed = 20;
        }
        else
        {
            moveSpeed = 10;
        }
        if (Input.GetAxis("Jump") != 0) IsSpeedUp = true;
        else IsSpeedUp = false;

    }

    void CheckSoundMove()
    {
        if (Input.GetAxis("Vertical") != 0) IsMoving = true;
        else IsMoving = false;
        if (IsMoving && !SoundWalk.isPlaying) SoundWalk.Play();
        if (!IsMoving) SoundWalk.Stop();
    }

    void CheckMove()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("IsWalk", true);
            EffectWalk_2.SetActive(true);
            EffectWalk_1.SetActive(true);
        }
        else
        {
            animator.SetBool("IsWalk", false);
            EffectWalk_2.SetActive(false);
            EffectWalk_1.SetActive(false);
        }
    }

    void CheckSoundSteer()
    {
        if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") == 0) IsSteering = true;
        else IsSteering = false;
        if (IsSteering && !SoundSteer.isPlaying) SoundSteer.Play();
        if (!IsSteering) SoundSteer.Stop();

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
            moveSpeed = 10;
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
            moveSpeed = 10;
        }


    }
}
