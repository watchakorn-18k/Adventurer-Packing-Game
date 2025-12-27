using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float destroyTime = 1.5f;
    public Vector3 motion = new Vector3(0, 1, 0);

    void Start()
    {
        // ทำลายตัวเองอัตโนมัติเมื่อครบเวลา
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // ลอยขึ้น
        transform.Translate(motion * moveSpeed * Time.deltaTime);
    }
}
