using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Delivery : MonoBehaviour
{
    [SerializeField] Color32 hasPackageColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 noPackageColor = new Color32(1, 1, 1, 1);
    [SerializeField] float ระยะเวลาทำลาย = 0.5f;

    int MaxPackageBlue = 5;
    int MaxPackageBlack = 5;
    bool hasPackage = false;
    bool isDelivering = false;
    public static bool isFullPlaceBlue;
    public static bool isFullPlaceBlack;
    public bool isFullPlaceAll;
    int AmoutOfPackage = 0;

    int AmoutOfPlaceBlue = 0;
    int AmoutOfPlaceBlack = 0;
    string ColorOfPlace = "";
    public static GameObject Cargo;
    public Text ScorePackage;
    public Text AmoutBlue;
    public Text AmoutBlack;

    GameObject PackageOnCar;
    GameObject CustomerPlace;


    [SerializeField] GameObject GameFinish;
    [SerializeField] GameObject UiFinishBlue;
    [SerializeField] GameObject UiFinishBlack;

    [SerializeField] Text TimerInGame;

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
        ScorePackage.text = AmoutOfPackage.ToString();



    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "packages" && !hasPackage)
        {
            // Destroy(other.gameObject, ระยะเวลาทำลาย);
            // Debug.Log("ตัวนี้มีของแล้ว");
            hasPackage = true;
            isDelivering = false;

            //ไปเรียกใช้งาน object ชื่อ Package เพื่อยกของขึ้นรถ
            // Debug.Log(other.gameObject.name);
            Cargo = GameObject.Find(other.gameObject.name);
            ScorePackage.text = AmoutOfPackage.ToString();

            //เรียกใช้งาน Package.cs
            Cargo.AddComponent<Package>();

        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "customerBlue" && hasPackage && AmoutOfPlaceBlue <= MaxPackageBlue - 1)
        {
            ColorOfPlace = "Blue";
            CustomerPlace = GameObject.Find(other.gameObject.name);
            hasPackage = false;
            isDelivering = true;


            //ลบ object กล่อง ที่อยู่บนรถ
            PackageOnCar = GameObject.Find($"{Cargo.name}");
            Destroy(PackageOnCar, ระยะเวลาทำลาย);


        }



        if (other.tag == "customerBlack" && hasPackage && AmoutOfPlaceBlack <= MaxPackageBlack - 1)
        {
            ColorOfPlace = "Black";
            CustomerPlace = GameObject.Find(other.gameObject.name);
            hasPackage = false;
            isDelivering = true;


            //ลบ object กล่อง ที่อยู่บนรถ
            PackageOnCar = GameObject.Find($"{Cargo.name}");
            Destroy(PackageOnCar, ระยะเวลาทำลาย);


        }




    }

    void ShowFinish()
    {
        Debug.Log("ครบทั้งสองแล้ว");
        Time.timeScale = 0f;
        GameFinish.SetActive(true);
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


            }
            if (AmoutOfPlaceBlack == MaxPackageBlack)
            {
                isFullPlaceBlack = true;
                UiFinishBlack.SetActive(true);
            }

        }


        if (isFullPlaceBlue && isFullPlaceBlack)
        {
            ShowFinish();

        }



    }


}
