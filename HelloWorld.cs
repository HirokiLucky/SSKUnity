using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 cv = new Vector3(0f, 1f, -5f);
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

        var mp = Input.mousePosition;
        var x = (int)(mp.x / (Screen.width / 3));
        var y = (int) (mp.y / (Screen.height / 3));

        var vx = Vector3.zero;
        var vy = Vector3.zero;
        var vz = Vector3.zero;
        switch (x)
        {
            case 0:
                vx = new Vector3(-1f, 0f, 0f);
                break;
            case 2:
                vx = new Vector3(1f, 0f, 0f);
                break;
        }

        switch (y)
        {
            case 0:
                vy = new Vector3(0f, 0f, -1f);
                break;
            case 2:
                vy = new Vector3(0f, 0f, 1f);
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            vz = new Vector3(0f, 1000f, 0f);
        }
        
            rb.AddForce(vx + vy + vz);
    }
}
