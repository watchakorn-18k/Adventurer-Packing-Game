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

    [Header("Effects")]
    [SerializeField] ParticleSystem dustPrefab; // ลากไฟล์ Prefab 'EffectWalk' จาก Project มาใส่ตรงนี้
    ParticleSystem currentDust;

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();

        // Setup Dust Effect
        if (dustPrefab != null)
        {
            // สร้าง Effect ขึ้นมาเป็นลูกของตัวละคร
            currentDust = Instantiate(dustPrefab, transform.position, Quaternion.identity);
            currentDust.transform.SetParent(transform);
            currentDust.transform.localPosition = new Vector3(0, -0.8f, 0); // ขยับไปไว้ด้านหลัง (หาง/เท้าคู่หลัง)
            currentDust.transform.localScale = Vector3.one;

            // ตั้งค่า Sorting Order ให้แสดงผลถูกต้อง
            var dustRenderer = currentDust.GetComponent<Renderer>();
            var playerRenderer = GetComponent<SpriteRenderer>();
            if (dustRenderer != null && playerRenderer != null)
            {
                dustRenderer.sortingLayerID = playerRenderer.sortingLayerID;
                dustRenderer.sortingOrder = playerRenderer.sortingOrder - 1; 
            }

            // ปรับแต่ง Particle System ให้ดูเป็นฝุ่นที่ทิ้งไว้ข้างหลัง (Trail)
            var main = currentDust.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World; // สำคัญ: ให้ฝุ่นอยู่ที่เดิม ไม่ตามตัวละครไป
            main.startLifetime = 0.5f; // หายไปเร็วขึ้น (0.5 วินาที)
            main.startSpeed = 0.2f;    // ไม่ต้องพุ่งไปไหนไกล
            main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.6f); // สุ่มขนาด
            main.maxParticles = 50;

            var emission = currentDust.emission;
            emission.rateOverTime = 20f; // ออกมาเยอะหน่อยจะได้ดูต่อเนื่อง


        }
    }

    [Header("Cargo Settings")]
    [SerializeField] Transform cargoContainer; // ลาก Object ที่เป็น Parent ของกล่องทั้งหมดมาใส่
    float cargoWobbleAmount = 0f;

    void Update()
    {
        // ... (Old Update Code) ...
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

        // อัปเดต Effect ฝุ่น (Dust Particles)
        if (currentDust != null)
        {
            if (isMoving && !currentDust.isPlaying)
            {
                currentDust.Play();
            }
            else if (!isMoving && currentDust.isPlaying)
            {
                currentDust.Stop();
            }
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

            // 3. เพิ่มลูกเล่นการเด้ง (Bobbing/Gallop Effect)
            float bobRange = 0.05f; 
            float bobSpeed = 15f;   
            
            float scaleChange = Mathf.Sin(Time.time * bobSpeed) * bobRange;
            transform.localScale = new Vector3(1 + scaleChange, 1 - scaleChange, 1);
        }
        else
        {
             transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 10f);
        }

        // --- Cargo Wobble Logic (กล่องโยกเยก) ---
        // หาเป้าหมายที่จะโยก (Visual Cargo หรือ กล่องที่เก็บมา)
        Transform targetCargo = cargoContainer;
        
        // ถ้าไม่มี Visual Cargo ให้ลองไปหาจาก Delivery Script (กล่องที่เก็บมา)
        if (targetCargo == null)
        {
            Delivery delivery = GetComponent<Delivery>();
            if (delivery != null && delivery.PackageOnCar != null)
            {
                targetCargo = delivery.PackageOnCar.transform;
            }
        }

        if (targetCargo != null)
        {
            // คำนวณแรงเหวี่ยง: ถ้าเลี้ยวซ้าย กล่องจะเอียงขวา (Inertia)
            // เลี้ยวแรงแค่ไหน ขึ้นอยู่กับ input แนวนอน (moveX)
            float targetWobble = -moveX * 15f; // เอียงสูงสุด 15 องศา
            
            // ทำให้กล่องค่อยๆ เอียงไปหาองศาเป้าหมาย (Lerp)
            cargoWobbleAmount = Mathf.Lerp(cargoWobbleAmount, targetWobble, Time.deltaTime * 5f);
            
            // หมุนเฉพาะแกน Z โดยอิงจาก Local Rotation เดิม
            // หมายเหตุ: ใช้ localRotation ถ้าเป็นลูก แต่ถ้ากล่องแยกอาจจะต้องดู Parent
            targetCargo.localRotation = Quaternion.Euler(0, 0, cargoWobbleAmount);
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
