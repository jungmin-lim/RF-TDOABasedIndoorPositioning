using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DController : MonoBehaviour
{
    public GameObject t;
    public Text text_d4;
    Vector3 tpos, pos;
    public static float d4;
    public float freq;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tpos = t.transform.position;
        tpos -= pos;
        d4 = tpos.magnitude;
        text_d4.text = "d4 = " + d4;
    }

    float massage_friis_equation(float d)
    {
        float temp;
        int ret;
        temp = 32.44f;
        temp += (float)(Math.Log10(d / 1000)) * 20;
        temp += (float)(Math.Log10(freq)) * 20;
        if (temp <= 0)
            temp = 0;
        ret = 17 - (int)(temp / 3 + 2);
        return (float)ret;
    }

    float sensor2d(float d)
    {
        float ret;
        ret = (float)Math.Pow(10, ((15 - d) * 3 / 20));
        ret = ret / (float)(4 * Math.PI) * 300 / freq;
        return ret;
    }
}
