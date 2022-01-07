using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Delivery : MonoBehaviour
{
    [SerializeField] float ระยะเวลาทำลาย = 0.5f;

    [SerializeField] GameObject GameFinish;
    [SerializeField] GameObject UiFinishBlue;
    [SerializeField] GameObject UiFinishBlack;

    [SerializeField] Text TimerInGame;


    public bool isFullPlaceAll;


    public int MaxPackageBlue; //Set value variable at Gameobject Tooy
    public int MaxPackageBlack; //Set value variable at Gameobject Tooy
    public AudioSource SoundMain;
    public AudioSource SoundPackage;

    public AudioSource SoundFinishSendPackage;

    public AudioSource SoundPlaceFull;

    public AudioSource CollisionBox;

    public static bool isFullPlaceBlue;
    public static bool isFullPlaceBlack;
    public static GameObject Cargo;


    bool hasPackage = false;
    bool isDelivering = false;
    int AmoutOfPackage = 0;
    int AmoutOfPackageInPlayer = 0;

    int AmoutOfPlaceBlue = 0;
    int AmoutOfPlaceBlack = 0;
    string ColorOfPlace = "";

    Text ScorePackage;
    Text AmoutBlue;
    Text AmoutBlack;

    GameObject PackageOnCar;
    GameObject CustomerPlace;



    TextMesh TxtReqBlue;
    TextMesh TxtReqBlack;


    //Import Component
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    Package package;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ScorePackage = GameObject.Find("ScorePackage").GetComponent<Text>();
        AmoutBlue = GameObject.Find("AmoutBlue").GetComponent<Text>();
        AmoutBlack = GameObject.Find("AmoutBlack").GetComponent<Text>();
        TxtReqBlue = GameObject.Find("FloatingTextBlue").GetComponent<TextMesh>();
        TxtReqBlack = GameObject.Find("FloatingTextBlack").GetComponent<TextMesh>();
        TxtReqBlue.text = $"{+AmoutOfPlaceBlue}/{MaxPackageBlue}";
        TxtReqBlack.text = $"{+AmoutOfPlaceBlack}/{MaxPackageBlack}";
        ScorePackage.text = AmoutOfPackage.ToString();



    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "packages" && !hasPackage)
        {
            SoundPackage.Play();
            hasPackage = true;
            isDelivering = false;
            AmoutOfPackageInPlayer += 1;

            //ไปเรียกใช้งาน object ชื่อ Package เพื่อยกของขึ้นรถ
            Cargo = GameObject.Find(other.gameObject.name);
            ScorePackage.text = AmoutOfPackage.ToString();


            //เรียกใช้งาน Package.cs
            Cargo.AddComponent<Package>();
        }

        if (other.gameObject.tag == "packages" && hasPackage)
        {
            CollisionBox.Play();
        }



    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "customerBlue" && hasPackage && AmoutOfPlaceBlue <= MaxPackageBlue - 1 && AmoutOfPackageInPlayer == 1)
        {
            ColorOfPlace = "Blue";
            CustomerPlace = GameObject.Find(other.gameObject.name);
            hasPackage = false;
            isDelivering = true;
            AmoutOfPackageInPlayer -= 1;


            //ลบ object กล่อง ที่อยู่บนรถ
            PackageOnCar = GameObject.Find($"{Cargo.name}");
            Destroy(PackageOnCar, ระยะเวลาทำลาย);
            SoundFinishSendPackage.Play();


        }



        if (other.tag == "customerBlack" && hasPackage && AmoutOfPlaceBlack <= MaxPackageBlack - 1 && AmoutOfPackageInPlayer == 1)
        {
            ColorOfPlace = "Black";
            CustomerPlace = GameObject.Find(other.gameObject.name);
            hasPackage = false;
            isDelivering = true;
            AmoutOfPackageInPlayer -= 1;


            //ลบ object กล่อง ที่อยู่บนรถ
            PackageOnCar = GameObject.Find($"{Cargo.name}");
            Destroy(PackageOnCar, ระยะเวลาทำลาย);
            SoundFinishSendPackage.Play();
        }




    }

    void ShowFinish()
    {
        Time.timeScale = 0f;
        GameFinish.SetActive(true);
        SoundMain.mute = true;
        Text ScorePackageMenu;
        Text TimerFinish;
        ScorePackageMenu = GameObject.Find("ScorePackageMenu").GetComponent<Text>();
        ScorePackageMenu.text = ScorePackage.text;
        TimerFinish = GameObject.Find("TimerFinish").GetComponent<Text>();
        TimerFinish.text = TimerInGame.text;
        // TimerInGame


    }

    void Update()
    {
        PlayerPrefs.SetInt("Score", AmoutOfPackage);





        if (ColorOfPlace == "Blue")
        {
            // เช็คถ้าของยังไม่ถูกทำลายจะให้รถไปอยู่ในตำแหน่ง CustomPlace
            if (isDelivering && PackageOnCar != null)
            {
                gameObject.transform.position = new Vector3(CustomerPlace.transform.position.x, CustomerPlace.transform.position.y, -5f);
            }

            // เช็คถ้าของถูกทำลายแล้ว
            if (isDelivering && PackageOnCar == null)
            {
                AmoutOfPlaceBlue += 1;
                AmoutOfPackage += 1;

                TxtReqBlue.text = $"{+AmoutOfPlaceBlue}/{MaxPackageBlue}";
                AmoutBlue.text = AmoutOfPlaceBlue.ToString();
                ScorePackage.text = AmoutOfPackage.ToString();
                isDelivering = false;
                if (AmoutOfPlaceBlue == MaxPackageBlue)
                {
                    isFullPlaceBlue = true;
                    UiFinishBlue.SetActive(true);
                    SoundPlaceFull.Play();
                }

            }
        }
        if (ColorOfPlace == "Black")
        {
            // เช็คถ้าของยังไม่ถูกทำลายจะให้รถไปอยู่ในตำแหน่ง CustomPlace
            if (isDelivering && PackageOnCar != null)
            {
                gameObject.transform.position = new Vector3(CustomerPlace.transform.position.x, CustomerPlace.transform.position.y, -5f);
            }

            // เช็คถ้าของถูกทำลายแล้ว
            if (isDelivering && PackageOnCar == null)
            {
                AmoutOfPlaceBlack += 1;
                AmoutOfPackage += 1;


                TxtReqBlack.text = $"{+AmoutOfPlaceBlack}/{MaxPackageBlack}";
                AmoutBlack.text = AmoutOfPlaceBlack.ToString();
                ScorePackage.text = AmoutOfPackage.ToString();
                isDelivering = false;
                if (AmoutOfPlaceBlack == MaxPackageBlack)
                {
                    isFullPlaceBlack = true;
                    UiFinishBlack.SetActive(true);
                    SoundPlaceFull.Play();
                }
            }
        }


        if (isFullPlaceBlue && isFullPlaceBlack)
        {
            ShowFinish();

        }



    }


}
