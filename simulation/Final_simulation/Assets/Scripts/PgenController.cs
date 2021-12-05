using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PgenController : MonoBehaviour
{
    public GameObject prefab;
    GameObject[] cinstance = new GameObject[10000];
    void Start()
    {

        for (int i = 0; i < 10000; i++)
        {
            cinstance[i] = Instantiate(prefab);
        }
            
    }

    void Update()
    {
        
    }
}
