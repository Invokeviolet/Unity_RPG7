using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    float degree;
    int speed;

    void Start()
    {
        degree = 0;
        speed = 1;
    }

    void Update()
    {
        degree += Time.deltaTime * speed;
        if (degree >= 360)
            degree = 0;

        RenderSettings.skybox.SetFloat("_Rotation", degree);
    }
}
