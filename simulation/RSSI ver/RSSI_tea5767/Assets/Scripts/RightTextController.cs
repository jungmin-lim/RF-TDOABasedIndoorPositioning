using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightTextController : MonoBehaviour
{
    public Text text_p123, text_c,text_t,text_et;
    public GameObject p1, p2, p3, c,t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text_p123.text = "p1=(" + p1.transform.position.x + ", " + p1.transform.position.z + ")\n" +
            "p2=(" + p2.transform.position.x + ", " + p2.transform.position.z + ")\n" +
            "p3=(" + p3.transform.position.x + ", " + p3.transform.position.z + ")\n";
        text_c.text = "c=(" + c.transform.position.x + ", " + c.transform.position.z + ")";
        text_t.text = "t=(" + t.transform.position.x + ", " + t.transform.position.z + ")";
        text_et.text = "e_t=("+(t.transform.position-c.transform.position).magnitude+")";
    }
}
