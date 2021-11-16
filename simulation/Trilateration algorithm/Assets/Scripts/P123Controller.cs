using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class P123Controller : MonoBehaviour
{
    public GameObject p1, p2, p3;
    float x_a = 0.0f, z_a = 0.0f;
    float x_b = 0.0f, z_b = 25.0f;
    float x_c = 25.0f, z_c = 0.0f;
    float x_d = 25.0f, z_d = 25.0f;
    Vector2 tempv2;
    Vector3 tempv3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempv2 = trilateration(x_a, z_a, x_c, z_c, x_b, z_b, AController.d1, CController.d3, BController.d2);
        tempv3 = new Vector3(tempv2.x, 0.0f, tempv2.y);
        p1.transform.position=tempv3;
        tempv2 = trilateration(x_a, z_a, x_d, z_d, x_b, z_b,  AController.d1, DController.d4, BController.d2);
        tempv3 = new Vector3(tempv2.x, 0.0f, tempv2.y);
        p2.transform.position = tempv3;
        tempv2 = trilateration(x_b, z_b, x_c, z_c, x_d, z_d, BController.d2, CController.d3, DController.d4);
        tempv3 = new Vector3(tempv2.x, 0.0f, tempv2.y);
        p3.transform.position = tempv3;
    }
    Vector2 trilateration(float x1, float z1, float x2, float z2, float x3, float z3, float r1, float r2, float r3)
    {
        Vector2 ret;
        float S = (float)((Math.Pow(x3, 2) - Math.Pow(x2, 2) + Math.Pow(z3, 2) - Math.Pow(z2, 2) + Math.Pow(r2, 2) - Math.Pow(r3, 2)) / 2.0f);
        float T = (float)((Math.Pow(x1, 2) - Math.Pow(x2, 2) + Math.Pow(z1, 2) - Math.Pow(z2, 2) + Math.Pow(r2, 2) - Math.Pow(r1, 2)) / 2.0f);
        ret.y = ((T * (x2 - x3)) - (S * (x2 - x1))) / (((z1 - z2) * (x2 - x3)) - ((z3 - z2) * (x2 - x1)));
        ret.x = ((ret.y * (z1 - z2)) - T) / (x2 - x1);

        return ret;
    }
}
