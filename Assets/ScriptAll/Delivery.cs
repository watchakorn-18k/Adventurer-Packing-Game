using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Delivery : MonoBehaviour
{
    [SerializeField] float ระยะเวลาทำลาย = 0.5f;

    [SerializeField] GameObject GameFinish;
    [SerializeField] GameObject UiFinishBlue;
    [SerializeField] GameObject UiFinishBlack;

    [SerializeField] Text TimerInGame;

    TimeSpan TimeSpan;


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

    // Navigator Variables
    GameObject navArrow;
    List<GameObject> blueCustomers = new List<GameObject>();
    List<GameObject> blackCustomers = new List<GameObject>();






    public bool hasPackage = false;
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

    public GameObject PackageOnCar;
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

        // Init Navigator
        blueCustomers.AddRange(GameObject.FindGameObjectsWithTag("customerBlue"));
        blackCustomers.AddRange(GameObject.FindGameObjectsWithTag("customerBlack"));
        CreateNavigatorArrow();
    }

    void Update()
    {
        PlayerPrefs.SetInt("Score", AmoutOfPackage);
        ScoreAll.text = checkScore();
        CheckPlaceAll();
        CheckNewHightScore();
        UpdateNavigator();
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
            // Destroy(PackageOnCar, ระยะเวลาทำลาย); // [Modified] เปลี่ยนเป็นอนิเมชั่นโยน
            StartCoroutine(AnimateThrow(PackageOnCar, CustomerPlace.transform.position));
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
            // Destroy(PackageOnCar, ระยะเวลาทำลาย); // [Modified] เปลี่ยนเป็นอนิเมชั่นโยน
            StartCoroutine(AnimateThrow(PackageOnCar, CustomerPlace.transform.position));
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
            // TimerFinish.text = TimerInGame.text;
            TimeSpan = TimeSpan.FromSeconds(Timer.CountDownStart - 400f);
            TimerFinish.text = $"{String.Format(@"{0:mm\:ss}", TimeSpan)}";

            // TimerInGame
        }


    }

    void CheckPlaceAll()
    {
        if (ColorOfPlace == "Blue")
        {
            // [Modified] ไม่ล็อกขาผู้เล่นแล้ว ให้โยนของแล้วขับไปต่อได้เลย (Drive-by)
            /*
            if (isDelivering && PackageOnCar != null)
            {
                gameObject.transform.position = new Vector3(CustomerPlace.transform.position.x, CustomerPlace.transform.position.y, -5f);
            }
            */

            // เช็คถ้าของถูกทำลายแล้ว
            if (isDelivering && PackageOnCar == null)
            {
                AmoutOfPlaceBlue += 1;
                AmoutOfPackage += 1;
                SaveScoreTotxt();

                // Show Floating Text (+1!)
                ShowFloatingText(CustomerPlace.transform.position, "+1!", Color.cyan);

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
            // [Modified] ไม่ล็อกขาผู้เล่นแล้ว
            /*
            if (isDelivering && PackageOnCar != null)
            {
                gameObject.transform.position = new Vector3(CustomerPlace.transform.position.x, CustomerPlace.transform.position.y, -5f);
            }
            */

            // เช็คถ้าของถูกทำลายแล้ว
            if (isDelivering && PackageOnCar == null)
            {
                AmoutOfPlaceBlack += 1;
                AmoutOfPackage += 1;
                SaveScoreTotxt();


                // Show Floating Text (+1!)
                ShowFloatingText(CustomerPlace.transform.position, "+1!", Color.red);

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




    void CreateNavigatorArrow()
    {
        navArrow = new GameObject("NavigatorArrow");
        navArrow.transform.SetParent(this.transform);
        navArrow.transform.localPosition = Vector3.zero;

        SpriteRenderer sr = navArrow.AddComponent<SpriteRenderer>();

        // Create Procedural Texture (Visual Fix)
        int w = 64;
        int h = 64;
        Texture2D tex = new Texture2D(w, h);
        tex.filterMode = FilterMode.Bilinear;
        Color[] colors = new Color[w * h];
        Color empty = new Color(0, 0, 0, 0);
        Color arrowColor = new Color(1f, 0.8f, 0f, 1f); // Golden Yellow
        Color outlineColor = Color.black;

        // Clear
        for (int i = 0; i < colors.Length; i++) colors[i] = empty;

        // Draw Arrow Logic
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                // Normalize coords centered at 32,32
                float nx = x - 32f;
                float ny = y - 32f;

                // Simple Arrow Shape Math
                bool inside = false;
                bool outline = false;

                // Stem: Width 12 (-6 to 6), Height from -20 to 5
                if (ny >= -20 && ny <= 5 && Mathf.Abs(nx) <= 6) inside = true;
                if (ny >= -21 && ny <= 6 && Mathf.Abs(nx) <= 7 && !inside) outline = true;

                // Head: Triangle base at y=5, tip at y=25. Width varies.
                // Height of triangle = 20.
                if (ny > 5 && ny < 25)
                {
                    float p = (25 - ny) / 20f; // 1 at base, 0 at tip. 
                    // Width at base = 30 (-15 to 15).
                    if (Mathf.Abs(nx) <= p * 18f) inside = true;
                    else if (Mathf.Abs(nx) <= p * 18f + 1.5f) outline = true;
                }
                
                // Color assignment
                if (inside) colors[y * w + x] = arrowColor;
                else if (outline) colors[y * w + x] = outlineColor;
            }
        }

        tex.SetPixels(colors);
        tex.Apply();

        Sprite arrowSprite = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f));
        sr.sprite = arrowSprite;
        
        navArrow.transform.localScale = new Vector3(4f, 4f, 1f);
        sr.sortingOrder = 99;
        navArrow.SetActive(false); // Hide by default
    }

    void UpdateNavigator()
    {
        // Safety check: if navArrow wasn't created (e.g. Start failed), try to create it or skip
        if (navArrow == null) 
        {
            CreateNavigatorArrow();
            if (navArrow == null) return; // Still null? Abort.
        }

        if (hasPackage)
        {
            GameObject target = GetBestTarget();
            if (target != null)
            {
                navArrow.SetActive(true);

                // Direction
                Vector3 dir = target.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                navArrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

                // Offset from player
                navArrow.transform.position = transform.position + dir.normalized * 4.5f;
            }
            else
            {
                navArrow.SetActive(false);
            }
        }
        else
        {
            navArrow.SetActive(false);
        }
    }

    GameObject GetBestTarget()
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 pos = transform.position;

        // Check Blue
        if (!isFullPlaceBlue)
        {
            foreach (var go in blueCustomers)
            {
                if (go == null) continue;
                float dist = Vector3.Distance(pos, go.transform.position);
                if (dist < minDist) { minDist = dist; closest = go; }
            }
        }

        // Check Black
        if (!isFullPlaceBlack)
        {
            foreach (var go in blackCustomers)
            {
                if (go == null) continue;
                float dist = Vector3.Distance(pos, go.transform.position);
                if (dist < minDist) { minDist = dist; closest = go; }
            }
        }
        return closest;
    }

    void ShowFloatingText(Vector3 position, string text, Color color)
    {
        GameObject go = new GameObject("FloatingText");
        go.transform.position = position + new Vector3(0, 1.5f, -2f);
        
        // Load Game Font (SP-Kangfu)
        Font gameFont = Resources.Load<Font>("Font/SP-Kangfu"); // Try loading if in Resources
        // If not in Resources, we'll try to find it from another object, but for now let's use Arial or fallback.
        // Actually, best way is to expose a public Font variable.
        // But since user asked to edit this function, I will add a text mesh component setup.
        
        // Setup Main Text
        SetupTextMesh(go, text, color, 180, 20, gameFont); // Increased size to 180
        
        // Create Outline (Simulated by 4 black texts behind)
        float offset = 0.1f;
        createOutlinePart(go, text, new Vector3(-offset, -offset, 0.1f), gameFont);
        createOutlinePart(go, text, new Vector3( offset, -offset, 0.1f), gameFont);
        createOutlinePart(go, text, new Vector3(-offset,  offset, 0.1f), gameFont);
        createOutlinePart(go, text, new Vector3( offset,  offset, 0.1f), gameFont);

        // Add Logic
        go.AddComponent<FloatingText>();
        go.transform.localScale = Vector3.one;
    }

    void createOutlinePart(GameObject parent, string text, Vector3 localOffset, Font font)
    {
        GameObject outline = new GameObject("Outline");
        outline.transform.SetParent(parent.transform);
        outline.transform.localPosition = localOffset;
        outline.transform.localScale = Vector3.one;
        SetupTextMesh(outline, text, Color.black, 180, 20, font);
    }

    void SetupTextMesh(GameObject go, string text, Color color, int fontSize, int sortOrder, Font font)
    {
        TextMesh tm = go.AddComponent<TextMesh>();
        tm.text = text;
        tm.color = color;
        tm.fontSize = fontSize;
        tm.characterSize = 0.1f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        if (font != null) tm.font = font;

        // Renderer
        MeshRenderer rn = go.GetComponent<MeshRenderer>();
        if (rn != null) 
        {
            rn.sortingOrder = sortOrder; // Main text = 20, Outline will inherit parent but we actually need to be careful.
            // Let's set Sorting Order explicitly.
            if (color == Color.black) rn.sortingOrder = sortOrder - 1; // Behind
            else rn.sortingOrder = sortOrder;
            
            // Fix Font Material issue (TextMesh needs Font Material)
            if (font != null) rn.material = font.material;
        }
    }

    IEnumerator AnimateThrow(GameObject pkg, Vector3 targetPos)
    {
        if (pkg == null) yield break;

        // แยกกล่องออกจากตัวรถ (จะได้บินอิสระ)
        pkg.transform.SetParent(null);

        Vector3 startPos = pkg.transform.position;
        float duration = 0.5f; // ใช้เวลาบิน 0.5 วินาที
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (pkg == null) yield break;
            
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // คำนวณตำแหน่ง Lerp แบบเส้นตรง
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            // เพิ่มความโค้ง (Parabola) แกน Y
            // สูตร: 4 * height * t * (1-t) จะได้รูปภูเขา (0 -> 1 -> 0)
            float arcHeight = 2.0f;
            currentPos.y += 4 * arcHeight * t * (1 - t);

            pkg.transform.position = currentPos;

            // หมุนติ้วๆ
            pkg.transform.Rotate(0, 0, 360 * Time.deltaTime * 2);

            // ย่อขนาดตอนใกล้ถึง
            pkg.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

            yield return null;
        }

        // ทำลายเมื่อถึงเป้าหมาย
        Destroy(pkg);
    }

}
