using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointCController : MonoBehaviour
{
    public GameObject p1, p2, p3;
    Vector3 temp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp = new Vector3(
            (p1.transform.position.x + p2.transform.position.x + p3.transform.position.x) / 3,
            (p1.transform.position.y + p2.transform.position.y + p3.transform.position.y) / 3,
            (p1.transform.position.z + p2.transform.position.z + p3.transform.position.z) / 3);
        transform.position = temp;
    }
}
