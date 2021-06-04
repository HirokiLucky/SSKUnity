using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    bool f = true;
    Vector3 cv = new Vector3(0f, 3f, -10f);
    Rigidbody rb = null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var sv = transform.position;
        sv.y = 1f;
        Camera.main.transform.position = sv + cv;

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var v = new Vector3(x, 0, y);

        rb.AddForce(v);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Other")
        {
            var ex = GameObject.Find("BigExplosion");
            var ps = ex.GetComponent<ParticleSystem>();
            ps.Play();
        }
    }
}
