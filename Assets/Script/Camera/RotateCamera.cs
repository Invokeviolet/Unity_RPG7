using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // 마우스커서에 따라 카메라 회전

    public float speed = 2.5f;
    private float xRotate = 0.0f;



    void Update()

    {
        // 좌우이동 * 속도
        float yRotateSize = Input.GetAxis("Mouse X") * speed;
        // y축 회전값 + 새로운 회전각도
        float yRotate = transform.eulerAngles.y + yRotateSize;
        // 위아래이동 * 속도
        float xRotateSize = -Input.GetAxis("Mouse Y") * speed;
        // 위아래 회전량 + 새로운 회전각도, 제한 범위
        // Clamp 값의 범위를 제한 -> min, max
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -40, 60);
        // 카메라 회전량 = 카메라에 반영
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
}
