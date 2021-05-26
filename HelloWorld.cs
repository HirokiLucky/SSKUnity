using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    int n = 0;
    Vector3 cv = new Vector3(0f, 1f, -5f);
    Rigidbody rb = null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var sv = transform.position + cv;
        Camera.main.transform.position = sv;
        if (n == 0)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            var v = new Vector3(0f, 0f, 200f);
            rb.AddForce(v);
        }

        n++;
        if (n == 2500)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            var v = new Vector3(0f, 1f, -200f);
            n = -2500;
            rb.AddForce(v);
        }
    }
}
