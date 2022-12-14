using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveTitleMap : MonoBehaviour
{
    //float speed = 0.1f;
    float time = 0;
    Vector3 myVec;
    Transform myTransform;
    void Update()
    {
        if (this.gameObject.transform.position.y <= 50) 
        {
            this.gameObject.transform.Translate(0, -0.006f, 0);
        }
        else if (this.gameObject.transform.position.y <= 40)
        {
            this.gameObject.transform.Translate(0, -0.004f, 0);
        }
        else if (this.gameObject.transform.position.y <= 30)
        {
            this.gameObject.transform.Translate(0, -0.002f, 0);
        }
        else
        {
            this.gameObject.transform.Translate(0, -0.008f, 0);
        }
        time += Time.deltaTime;
        

        if (this.gameObject.transform.position.y <= 28)
        {
            myVec = new Vector3(transform.position.x, 28, transform.position.z);
            this.gameObject.transform.position = myVec;
        }
    }
}
