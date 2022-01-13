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

    public GameObject ShowKeyboardSpace;
    public GameObject ShowKeyboardArrow;

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

    bool touchStart = false;

    Vector2 PointA;
    Vector2 PointB;

    float speed = 10f;




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
        JotStick();
        CheckHoldSpace();
        CheckSoundMove();
        CheckSoundSteer();
        isFullPlaceBlue = Delivery.isFullPlaceBlue;
        isFullPlaceBlack = Delivery.isFullPlaceBlack;
        CheckMove();
        ChecKArroKey();




    }

    void JotStick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            PointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
        }

    }

    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = PointB - PointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 500.0f);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);


        }
    }



    void ChecKArroKey()
    {
        if (Input.GetKey("up"))
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowUp", true);

        }
        else
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowUp", false);
        }

        if (Input.GetKey("down"))
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowDown", true);

        }
        else
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowDown", false);

        }

        if (Input.GetKey("left"))
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowLeft", true);

        }
        else
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowLeft", false);

        }

        if (Input.GetKey("right"))
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowRight", true);

        }
        else
        {
            ShowKeyboardArrow.GetComponent<Animator>().SetBool("ArrowRight", false);

        }
    }

    void CheckHoldSpace()
    {


        if (SpeedUp > 0)
        {
            moveSpeed = 20;
            ShowKeyboardSpace.GetComponent<Animator>().SetBool("IsIdleSpace", true);
        }
        else
        {
            moveSpeed = 10;
            ShowKeyboardSpace.GetComponent<Animator>().SetBool("IsIdleSpace", false);

        }
        if (Input.GetAxis("Jump") > 0.20f) ShowKeyboardSpace.GetComponent<Animator>().SetBool("HoldSpace", true);//IsSpeedUp = true
        else ShowKeyboardSpace.GetComponent<Animator>().SetBool("HoldSpace", false);

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
