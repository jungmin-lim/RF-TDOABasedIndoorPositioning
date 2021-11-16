using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TController : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed = 40.0f;

    private float h, v;
    void Start()
    {
        transform.position.Set((float)12.5, 0, (float)12.5);

    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical"); ;
        if ((transform.position.x > 0 && v>0) || (transform.position.x < 25 && v<0))
            transform.Translate(Vector3.left * v * moveSpeed * Time.deltaTime);
        if ((transform.position.z > 0 && h <0) || (transform.position.z < 25 && h > 0))
            transform.Translate(Vector3.forward * h * moveSpeed * Time.deltaTime);


    }
}
