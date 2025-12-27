using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    // นี่คือตำแหน่งกล้อง ควรจะอยู่ในตำแหน่งเดียวกับตำแหน่งรถ
    [SerializeField] GameObject สิ่งที่ต้องการติดตาม;


    public Animator Camera_animation;

    void Start()
    {
        Camera_animation.SetBool("IsZoom", true);

    }
    // Smooth Camera Settings
    [SerializeField] float smoothTime = 0.2f; // ระยะเวลาในการเคลื่อนที่ (ยิ่งมากยิ่งนุ่มนวล/ช้า)
    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // 1. หาตำแหน่งเป้าหมาย
        Vector3 targetPos = สิ่งที่ต้องการติดตาม.transform.position + new Vector3(0, 0, -10);

        // 2. คำนวณ Smooth Damp (ค่อยๆ เคลื่อนที่ไปหาเป้าหมาย)
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // 4. อัปเดตตำแหน่งกล้อง
        transform.position = smoothedPos;
    }
}
