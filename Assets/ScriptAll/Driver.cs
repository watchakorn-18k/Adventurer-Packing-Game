using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [Header("Buffalo Movement Settings")]
    [SerializeField] float moveSpeed = 15f;    // ความเร็วในการวิ่ง
    [SerializeField] float turnSpeed = 10f;    // ความไวในการหันหน้า (ยิ่งมากยิ่งหันไว)
    [SerializeField] float slowSpeed = 10f;    // ความเร็วเมื่อชน
    [SerializeField] float boostSpeed = 25f;    // ตัวแปรสำหรับเก็บความเร็วปัจจุบัน

    float currentMoveSpeed;
    
    // Animator Component
    Animator animator;

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // รับค่าปุ่มกดเป็นทิศทาง (Vector)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // สร้าง Vector ทิศทางรวม
        Vector2 moveDirection = new Vector2(moveX, moveY);
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;

        // อัปเดต Animation (Animator Controller)
        if (animator != null)
        {
            animator.SetBool("IsWalk", isMoving);
        }

        // ถ้ามีการกดปุ่ม (มีการเคลื่อนที่)
        if (isMoving)
        {
            // 1. การเคลื่อนที่ (Move)
            transform.Translate(moveDirection * currentMoveSpeed * Time.deltaTime, Space.World);

            // 2. การหันหน้า (Rotate)
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, -targetAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // 3. เพิ่มลูกเล่นการเด้ง (Bobbing/Gallop Effect) เพื่อให้ดูมีความเร็ว
            // ใช้ Sin wave ปรับ Scale แกน X และ Y สลับกันนิดหน่อย
            float bobRange = 0.05f; // ความแรงในการยืดหด (น้อยๆ ก็พอ)
            float bobSpeed = 15f;   // ความถี่ในการเด้ง (วิ่งเร็วก็เด้งรัว)
            
            float scaleChange = Mathf.Sin(Time.time * bobSpeed) * bobRange;
            // ให้แกน Y ยืดออก และ X หดเข้า สลับกัน (Squash & Stretch)
            transform.localScale = new Vector3(1 + scaleChange, 1 - scaleChange, 1);
        }
        else
        {
            // คืนค่า Scale ปกติเมื่อหยุดวิ่ง
             transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 10f);
        }
    }

    // --- ส่วนของการชนและลดความเร็ว (เหมือนเดิม) ---
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boost")
        {
            Debug.Log("Buffalo Boost!");
            currentMoveSpeed = boostSpeed;
            CancelInvoke("ResetSpeed");
            Invoke("ResetSpeed", 2f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // ไม่ลดความเร็วถ้าชนกล่องพัสดุหรือจุดส่งของ
        if (other.gameObject.tag == "packages" || other.gameObject.tag == "customerBlue" || other.gameObject.tag == "customerBlack")
        {
            return;
        }

        currentMoveSpeed = slowSpeed;
        CancelInvoke("ResetSpeed");
        Invoke("ResetSpeed", 1f);
    }

    void ResetSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }
}
