using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EController : MonoBehaviour
{
    public GameObject t;
    public Text text_d5;
    Vector3 tpos, pos;
    public static float d5;
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
        d5 = tpos.magnitude;
        d5 = massage_friis_equation(d5);
        d5 = sensor2d(d5) * 1.32933f;
        text_d5.text = "d5 = " + d5;

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
