using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

    public GameObject NewHightScore;

    public Animator Camera_animation;






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
    Text ScoreAll;

    GameObject PackageOnCar;
    GameObject CustomerPlace;



    TextMesh TxtReqBlue;
    TextMesh TxtReqBlack;

    Animator NewHightScore_animation;



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
        ScoreAll = GameObject.Find("ScoreAll").GetComponent<Text>();
        TxtReqBlue = GameObject.Find("FloatingTextBlue").GetComponent<TextMesh>();
        TxtReqBlack = GameObject.Find("FloatingTextBlack").GetComponent<TextMesh>();
        NewHightScore_animation = NewHightScore.GetComponent<Animator>();
        TxtReqBlue.text = $"{+AmoutOfPlaceBlue}/{MaxPackageBlue}";
        TxtReqBlack.text = $"{+AmoutOfPlaceBlack}/{MaxPackageBlack}";
        ScorePackage.text = AmoutOfPackage.ToString();
        NewHightScore_animation.SetBool("IsClose", false);



    }

    void Update()
    {
        PlayerPrefs.SetInt("Score", AmoutOfPackage);
        ScoreAll.text = checkScore();
        CheckPlaceAll();
        CheckNewHightScore();
        // PlayerPrefs.DeleteAll();












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



        StartCoroutine(WaitForAnimation());
        IEnumerator WaitForAnimation()
        {
            Camera_animation.SetBool("IsZoom", false);
            yield return new WaitForSeconds(0.3f);
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


    }

    void CheckPlaceAll()
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
                SaveScoreTotxt();

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
                SaveScoreTotxt();


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

    void SaveScoreTotxt()
    {
        string pathScore = Application.dataPath + "/Resources/SaveScore.txt";
        string pathName = Application.dataPath + "/Resources/SaveName.txt";
        File.AppendAllText(pathName, PlayerPrefs.GetString("SaveName") + "\n");

        File.AppendAllText(pathScore, $"{int.Parse(checkScore()) + 1}" + "\n");
    }

    string checkScore()
    {
        string path = Application.dataPath + "/Resources/SaveScore.txt";
        string[] lines = System.IO.File.ReadAllLines(path);
        string ScoreTxt = lines[lines.Length - 1];
        return ScoreTxt;
    }

    void CheckNewHightScore()
    {

        int ScoreCurrent = int.Parse(checkScore());
        int highscore = PlayerPrefs.GetInt("HighScore");

        if (highscore == 0)
        {
            NewHightScore.SetActive(false);

        }
        else
        {
            if (ScoreCurrent > highscore)
            {
                NewHightScore.SetActive(true);
                StartCoroutine(WaitForSecondsTime());

                IEnumerator WaitForSecondsTime()
                {
                    yield return new WaitForSeconds(60);
                    NewHightScore_animation.SetBool("IsClose", true);
                }

            }

        }


    }


}
