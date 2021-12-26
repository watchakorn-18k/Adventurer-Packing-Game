using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    // นี่คือตำแหน่งกล้อง ควรจะอยู่ในตำแหน่งเดียวกับตำแหน่งรถ
    [SerializeField] GameObject สิ่งที่ต้องการติดตาม;


    void LateUpdate()
    {

        transform.position = สิ่งที่ต้องการติดตาม.transform.position + new Vector3(0, 0, -10);
    }
}
