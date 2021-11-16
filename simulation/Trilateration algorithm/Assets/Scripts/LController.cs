using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LController : MonoBehaviour
{
    public float freq;
    public Text text_L;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text_L.text = "LA = " + friis_equation(AController.d1) + " LB = " + friis_equation(BController.d2)+ "\nLC = " + friis_equation(CController.d3)+ " LD = " + friis_equation(DController.d4);
    }

    float friis_equation(float d)
    {
        float ret;
        ret = 32.44f;
        ret += (float)(Math.Log10(d/1000))*20;
        ret += (float)(Math.Log10(freq)) * 20;
        return ret;
    }
}
