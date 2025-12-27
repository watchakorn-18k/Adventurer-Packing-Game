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
    LineRenderer rangeLine; // เส้นประวงกลม
    List<GameObject> blueCustomers = new List<GameObject>();
    List<GameObject> blackCustomers = new List<GameObject>();

    // [New] Stack counters for each customer (key = customer instance ID)
    Dictionary<int, int> customerStackCount = new Dictionary<int, int>();






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
        SetupRangeIndicator(); // สร้างเส้นประวงกลม
    }

    void Update()
    {
        PlayerPrefs.SetInt("Score", AmoutOfPackage);
        ScoreAll.text = checkScore();
        CheckPlaceAll();
        CheckNewHightScore();
        UpdateNavigator();
        CheckManualThrow(); // [New] เช็คการคลิกขว้างของ
        UpdateAimingLine(); // [New] เส้นเล็งเมาส์
    }

    // [New] อัปเดตเส้น Dash Line ตามเมาส์
    void UpdateAimingLine()
    {
        if (rangeLine == null) return;

        if (!hasPackage)
        {
            rangeLine.gameObject.SetActive(false);
            return;
        }

        rangeLine.gameObject.SetActive(true);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -5f; // [Fix] ดึงเส้นมาอยู่ข้างหน้าสุด
        
        Vector3 startPos = transform.position;
        startPos.z = -5f; // [Fix] ดึงจุดเริ่มมาข้างหน้าด้วย

        rangeLine.SetPosition(0, startPos);
        rangeLine.SetPosition(1, mousePos);

        Vector3 dir = (mousePos - startPos).normalized;
        float dist = Vector3.Distance(new Vector2(startPos.x, startPos.y), new Vector2(mousePos.x, mousePos.y));

        // Limit distance to 12m
        if (dist > 12f)
        {
            dist = 12f;
            mousePos = startPos + (dir * dist);
            // Re-apply Z
            mousePos.z = -5f; 
            rangeLine.SetPosition(1, mousePos);
        }

        // Adjust Tiling based on distance (ให้เส้นประความถี่เท่าเดิมตลอด)
        if (rangeLine.material != null)
        {
            rangeLine.material.mainTextureScale = new Vector2(dist, 1f); 
        }

        // [Modified] ไม่เปลี่ยนสีแล้ว ให้เป็นสีขาวตลอด (ผู้เล่นต้องกะเอง)
        rangeLine.startColor = Color.white;
        rangeLine.endColor = new Color(1, 1, 1, 0.5f);
    }

    // [New] ฟังก์ชันเช็คการคลิกเมาส์เพื่อโยนของ
    void CheckManualThrow()
    {
        if (Input.GetMouseButtonDown(0) && hasPackage)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);

            float dist = Vector2.Distance(playerPos, mousePos2D);

            // [Modified] โยนได้ทุกระยะ แต่ถ้าเกิน 12 เมตร จะ Clamp ตำแหน่งเป้าหมายให้อยู่แค่ 12 เมตร
            Vector3 targetThrowPos = mousePos;
            if (dist > 12f)
            {
                Vector2 dir = (mousePos2D - playerPos).normalized;
                Vector2 clampedPos = playerPos + (dir * 12f);
                mousePos2D = clampedPos;
                targetThrowPos = new Vector3(clampedPos.x, clampedPos.y, mousePos.z);
            }

            bool isHit = false;
            
            // [Modified] ใช้ OverlapCircle แทน Raycast จุดเดียว เพื่อให้เล็งง่ายขึ้น (รัศมี 1 เมตร)
            // ถ้าคลิกแถวๆ ลูกค้าในระยะ ก็ให้ถือว่าโดน (Forgiving Aim)
            Collider2D hitCol = Physics2D.OverlapCircle(mousePos2D, 1.0f);
            
            if (hitCol != null)
            {
                if (hitCol.CompareTag("customerBlue") && AmoutOfPlaceBlue < MaxPackageBlue)
                {
                    TryDeliver(hitCol.gameObject);
                    isHit = true;
                }
                else if (hitCol.CompareTag("customerBlack") && AmoutOfPlaceBlack < MaxPackageBlack)
                {
                    TryDeliver(hitCol.gameObject);
                    isHit = true;
                }
            }

            // ถ้าเล็งไม่โดนลูกค้า หรือลูกค้าเต็ม -> ปาพลาด (เสียหาย)
            if (!isHit)
            {
                ProcessMiss(targetThrowPos);
            }
        }
    }

    void ProcessMiss(Vector3 targetPos)
    {
        hasPackage = false;
        AmoutOfPackageInPlayer -= 1;

        // Visual Package Throw
        if (Cargo != null) 
        {
             // เก็บ Reference ไว้ใช้กับ Coroutine
             GameObject pkgThrow = GameObject.Find($"{Cargo.name}");
             
             // [Fix] ตัดการเชื่อมต่อทันที เพื่อไม่ให้ระบบอื่นตามเจอ
             PackageOnCar = null; 
             Cargo = null;

             if (pkgThrow != null)
             {
                 StartCoroutine(AnimateThrowMissNew(pkgThrow, targetPos));
             }
        }

        // Score Penalty (-1)

        // Score Penalty (-1)
        AmoutOfPackage -= 1; 
        SaveScoreTotxt();
        ScorePackage.text = AmoutOfPackage.ToString();
        
        // Show Text
        // ShowFloatingText(targetPos, "-1 Miss!", Color.red); // Moved to Animation
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
        TryDeliver(other.gameObject);
    }

    // [New] ย้าย Logic การส่งของมาไว้ตรงนี้ เพื่อให้เรียกใช้ได้ทั้งจาก Trigger และ Mouse Click
    void TryDeliver(GameObject customer)
    {
        if (!hasPackage || AmoutOfPackageInPlayer != 1) return;

        if (customer.tag == "customerBlue" && AmoutOfPlaceBlue <= MaxPackageBlue - 1)
        {
            ColorOfPlace = "Blue";
            CustomerPlace = customer;
            ProcessDelivery();
        }
        else if (customer.tag == "customerBlack" && AmoutOfPlaceBlack <= MaxPackageBlack - 1)
        {
            ColorOfPlace = "Black";
            CustomerPlace = customer;
            ProcessDelivery();
        }
    }

    void ProcessDelivery()
    {
        hasPackage = false;
        isDelivering = true;
        AmoutOfPackageInPlayer -= 1;

        //ลบ object กล่อง ที่อยู่บนรถ
        if (Cargo != null) 
        {
             PackageOnCar = GameObject.Find($"{Cargo.name}");
             // ถ้าหาไม่เจอ ให้ลองหาลูกของ CargoContainer (จาก Driver) หรือใช้วิธีอื่น
             // แต่ code เดิมใช้ GameObject.Find(Cargo.name) ซึ่งเสี่ยงถ้า name ซ้ำ แต่เราจะคง Logic เดิมไว้
             if (PackageOnCar != null)
             {
                 // [Modified] ใช้ AnimateThrowToStack แทน เพื่อวางกล่องซ้อนกัน
                 StartCoroutine(AnimateThrowToStack(PackageOnCar, CustomerPlace));
             }
        }
        
        SoundFinishSendPackage.Play();
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
        // [WebGL Fix] ใช้ PlayerPrefs แทน File I/O
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        currentScore++;
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save(); // บังคับ Save ทันที (สำคัญสำหรับ WebGL)
    }

    string checkScore()
    {
        // [WebGL Fix] อ่านจาก PlayerPrefs แทน File
        int score = PlayerPrefs.GetInt("CurrentScore", 0);
        return score.ToString();
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

    void SetupRangeIndicator()
    {
        GameObject rangeObj = new GameObject("RangeIndicator");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        rangeLine = rangeObj.AddComponent<LineRenderer>();
        rangeLine.useWorldSpace = true; // [Fix] ต้องใช้ World Space เพราะเราคำนวณตำแหน่งแบบ World
        rangeLine.sortingOrder = 50;    // [Fix] ให้แสดงผลทับทุกอย่าง
        rangeLine.loop = false;
        rangeLine.positionCount = 2;
        rangeLine.startWidth = 0.2f; // [Fix] เพิ่มความหนา
        rangeLine.endWidth = 0.2f;
        
        // Procedural Dashed Texture (32px: 16 white, 16 clear)
        Texture2D tex = new Texture2D(32, 1);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Repeat;
        for (int i = 0; i < 32; i++) 
        {
             if (i < 16) tex.SetPixel(i, 0, Color.white);
             else tex.SetPixel(i, 0, Color.clear);
        }
        tex.Apply();
        
        // Material setup
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.mainTexture = tex;
        rangeLine.material = mat;
        
        // Tile texture to create dashes
        rangeLine.textureMode = LineTextureMode.Tile; 
        rangeLine.material.mainTextureScale = new Vector2(1f, 1f);
        
        rangeLine.startColor = Color.white;
        rangeLine.endColor = Color.white;

        // Configure as simple line (Start -> End)
        rangeLine.positionCount = 2;
        rangeLine.loop = false;
        rangeObj.SetActive(false);
    }

    IEnumerator AnimateThrowMiss(GameObject pkg, Vector3 targetPos)
    {
        if (pkg == null) yield break;

        // [Fix] ปลด Parent
        pkg.transform.SetParent(null);
        
        // [Fix] ลบ Component ที่อาจจะดึงตำแหน่งกลับ
        Component[] components = pkg.GetComponents<MonoBehaviour>();
        foreach (var c in components) { Destroy(c); }

        // ปิด Collider และ Physics
        Collider2D col = pkg.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Rigidbody2D rb = pkg.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // [Fix] Force Sorting Order High & Force Z
        SpriteRenderer sr = pkg.GetComponent<SpriteRenderer>();
        if (sr == null) sr = pkg.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 100;

        Vector3 startPos = pkg.transform.position;
        targetPos.z = startPos.z; // Force Z match
        float duration = 0.5f; // เวลาในการลอย
        float elapsed = 0f;

        // 1. ช่วงลอยกลางอากาศ (Fly) - Parabola
        while (elapsed < duration)
        {
            if (pkg == null) yield break;
            
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Parabola Move
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            float arcHeight = 2.0f; 
            currentPos.y += 4 * arcHeight * t * (1 - t); 
            currentPos.z = startPos.z; // [Fix] Maintain Z
            pkg.transform.position = currentPos;

            // Spin
            pkg.transform.Rotate(0, 0, 360 * Time.deltaTime * 3);
            yield return null;
        }

        // 2. ช่วงตกกระแทกและกลิ้ง (Hit Ground & Roll)
        pkg.transform.position = targetPos; // ถึงพื้นแล้ว
        
        float rollDuration = 1.5f; // [Modified] กลิ้งนานขึ้นหน่อย (1.5 วิ)
        float rollElapsed = 0f;
        
        // สุ่มทิศทางกลิ้งเล็กน้อย
        Vector3 rollDir = (targetPos - startPos).normalized; 

        while (rollElapsed < rollDuration)
        {
            if (pkg == null) yield break;
            rollElapsed += Time.deltaTime;
            float t = rollElapsed / rollDuration; 

            // กลิ้งไปข้างหน้า (EaseOut - เร็วแล้วค่อยๆ หยุด)
            float speed = 2.0f * (1 - t); 
            Vector3 nextPos = pkg.transform.position + (rollDir * speed * Time.deltaTime);
            nextPos.z = startPos.z; // [Fix] Maintain Z
            pkg.transform.position = nextPos;

            // หมุนติ้วๆ
            pkg.transform.Rotate(0, 0, -360 * speed * Time.deltaTime);

            // Fade Out (ให้เริ่มจางเมื่อผ่านไปครึ่งทางแล้ว)
            if (t > 0.5f && sr != null)
            {
                float fadeT = (t - 0.5f) / 0.5f; // 0 -> 1 ในครึ่งหลัง
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, fadeT); 
                sr.color = c;
            }

            yield return null;
        }

        Destroy(pkg);
    }

    IEnumerator AnimateThrowMissNew(GameObject pkg, Vector3 targetPos)
    {
        if (pkg == null) yield break;

        // 1. Setup: Detach and Cleanup
        pkg.transform.SetParent(null);
        foreach (var c in pkg.GetComponents<MonoBehaviour>()) { Destroy(c); }
        
        Collider2D col = pkg.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        Rigidbody2D rb = pkg.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        SpriteRenderer sr = pkg.GetComponent<SpriteRenderer>();
        if (sr == null) sr = pkg.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 100;

        // 2. Fly Phase (Parabola)
        Vector3 startPos = pkg.transform.position;
        targetPos.z = startPos.z; // Fix Z
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (pkg == null) yield break;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            currentPos.y += 4 * 2.0f * t * (1 - t); // Arc
            currentPos.z = startPos.z;
            pkg.transform.position = currentPos;

            pkg.transform.Rotate(0, 0, 360 * Time.deltaTime * 3);
            yield return null;
        }

        // 3. Roll Phase (Decelerate to Stop)
        pkg.transform.position = targetPos;
        float rollDuration = 1.0f; // Roll for 1s
        float rollElapsed = 0f;
        Vector3 rollDir = (targetPos - startPos).normalized;
        float initialSpeed = 2.0f;

        while (rollElapsed < rollDuration)
        {
            if (pkg == null) yield break;
            rollElapsed += Time.deltaTime;
            float t = rollElapsed / rollDuration;

            // Speed from 2.0 -> 0.0
            float currentSpeed = initialSpeed * (1f - t); 
            
            Vector3 nextPos = pkg.transform.position + (rollDir * currentSpeed * Time.deltaTime);
            nextPos.z = startPos.z;
            pkg.transform.position = nextPos;

            pkg.transform.Rotate(0, 0, -360 * currentSpeed * 0.5f * Time.deltaTime);
            yield return null;
        }

        // 4. Fade Phase (Static)
        // [New] Show Penalty Text when stopped
        ShowFloatingText(targetPos, "-1", Color.red);

        float fadeDuration = 0.5f;
        float fadeElapsed = 0f;

        if (sr != null)
        {
            Color startColor = sr.color;
            while (fadeElapsed < fadeDuration)
            {
                if (pkg == null) yield break;
                fadeElapsed += Time.deltaTime;
                float t = fadeElapsed / fadeDuration;

                Color c = startColor;
                c.a = Mathf.Lerp(1f, 0f, t);
                sr.color = c;
                yield return null;
            }
        }

        Destroy(pkg);
    }

    // [New] Animation for successful delivery - Stacks boxes next to customer
    IEnumerator AnimateThrowToStack(GameObject pkg, GameObject customer)
    {
        if (pkg == null || customer == null) yield break;

        // แยกกล่องออกจากตัวรถ
        pkg.transform.SetParent(null);
        
        // ปิด Script/Collider ที่อาจจะรบกวน
        foreach (var c in pkg.GetComponents<MonoBehaviour>()) { Destroy(c); }
        Collider2D col = pkg.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Rigidbody2D rb = pkg.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // คำนวณตำแหน่งวางกล่อง (ข้างๆ ลูกค้า + ซ้อนกันตามจำนวน)
        int customerId = customer.GetInstanceID();
        if (!customerStackCount.ContainsKey(customerId))
        {
            customerStackCount[customerId] = 0;
        }
        int stackIndex = customerStackCount[customerId];
        customerStackCount[customerId]++;

        // ตำแหน่งฐาน: ขยับไปทางขวาของลูกค้า 1.5 หน่วย
        // ตำแหน่งสูง: แต่ละกล่องสูงขึ้น 0.5 หน่วย
        float boxHeight = 0.5f;
        float offsetX = 1.5f;
        Vector3 stackPos = customer.transform.position + new Vector3(offsetX, stackIndex * boxHeight, 0);
        stackPos.z = pkg.transform.position.z; // คงค่า Z

        Vector3 startPos = pkg.transform.position;
        float duration = 0.4f;
        float elapsed = 0f;

        // 1. Fly Phase (Parabola)
        while (elapsed < duration)
        {
            if (pkg == null) yield break;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, stackPos, t);
            float arcHeight = 1.5f;
            currentPos.y += 4 * arcHeight * t * (1 - t);
            currentPos.z = startPos.z;
            pkg.transform.position = currentPos;

            pkg.transform.Rotate(0, 0, 360 * Time.deltaTime * 2);
            yield return null;
        }

        // 2. วางลงพื้น (ปรับขนาดกลับเป็นปกติ และหยุดนิ่ง)
        pkg.transform.position = stackPos;
        pkg.transform.rotation = Quaternion.identity; // หยุดหมุน ตั้งตรง
        pkg.transform.localScale = Vector3.one * 0.8f; // ย่อนิดหน่อยให้ดูเป็นระเบียบ

        // [Optional] Force Sorting Order ให้กล่องที่สูงกว่าอยู่หน้า
        SpriteRenderer sr = pkg.GetComponent<SpriteRenderer>();
        if (sr == null) sr = pkg.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 10 + stackIndex; // กล่องที่สูงกว่าจะอยู่หน้า
        }

        // [Fix] ตั้ง PackageOnCar = null เพื่อให้ระบบนับคะแนนทำงาน
        // (เพราะ Update() เช็คว่า PackageOnCar == null ถึงจะนับ)
        PackageOnCar = null;
    }
}
