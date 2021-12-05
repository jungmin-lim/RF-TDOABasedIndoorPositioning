using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class P123Controller : MonoBehaviour
{
    public GameObject p1, p2, p3;
    public float speed_of_signal,bias;
    public float n;
    float rand_val1, rand_val2, rand_val3, rand_val4,vari;
    float x_a = 0.0f, z_a = 0.0f;
    float x_b = 0.0f, z_b = 25.0f;
    float x_c = 25.0f, z_c = 0.0f;
    float x_d = 25.0f, z_d = 25.0f;
    float d1, d2, d3, d4;
    private static UnityEngine.Random.State seedGenerator;
    private static int seedGeneratorSeed = 1337;
    private static bool seedGeneratorInitialized = false;
    Vector2 tempv2;
    Vector3 tempv3;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(GenerateSeed());
        rand_val1 = NextGaussian();
        rand_val2 = NextGaussian();
        rand_val3 = NextGaussian();
        rand_val4 = NextGaussian();
    }

    // Update is called once per frame
    void Update()
    {
        d1 = 1.96f * rand_val1 * (speed_of_signal * bias / (4*Mathf.Sqrt(3*n))) - 0.5f + AController.d1;
        d2 = 1.96f * rand_val2 * (speed_of_signal * bias / (4*Mathf.Sqrt(3*n))) - 0.5f + BController.d2;
        d3 = 1.96f * rand_val3 * (speed_of_signal * bias / (4*Mathf.Sqrt(3*n))) - 0.5f + CController.d3;
        d4 = 1.96f * rand_val4 * (speed_of_signal * bias / (4*Mathf.Sqrt(3*n))) - 0.5f + DController.d4;
        tempv2 = trilateration(x_a, z_a, x_c, z_c, x_b, z_b, d1, d3, d2);
        tempv3 = new Vector3(tempv2.x, 0.0f, tempv2.y);
        p1.transform.position=tempv3;
        tempv2 = trilateration(x_a, z_a, x_d, z_d, x_b, z_b,  d1, d4, d2);
        tempv3 = new Vector3(tempv2.x, 0.0f, tempv2.y);
        p2.transform.position = tempv3;
        tempv2 = trilateration(x_b, z_b, x_c, z_c, x_d, z_d, d2, d3, d4);
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

    public static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);
        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    public static int GenerateSeed()
    {
        // remember old seed
        var temp = UnityEngine.Random.state;

        // initialize generator state if needed
        if (!seedGeneratorInitialized)
        {
            UnityEngine.Random.InitState(seedGeneratorSeed);
            seedGenerator = UnityEngine.Random.state;
            seedGeneratorInitialized = true;
        }

        // set our generator state to the seed generator
        UnityEngine.Random.state = seedGenerator;
        // generate our new seed
        var generatedSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        // remember the new generator state
        seedGenerator = UnityEngine.Random.state;
        // set the original state back so that normal random generation can continue where it left off
        UnityEngine.Random.state = temp;

        return generatedSeed;
    }
}
