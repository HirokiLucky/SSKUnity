using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerOfCharacter : MonoBehaviour
{
    public GameObject[] target;
    public float distance;
    private Vector3 d;
    
    // Start is called before the first frame update
    void Start()
    {
        d = Vector3.forward * distance + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 allp = Vector3.zero;
        foreach (var ob in target)
        {
            allp += ob.transform.position;
        }

        var p = allp / target.Length;
        p.y = 0f;
        p.z = -5f;
        transform.position = p + d;
        d = transform.position - p;
        d.y = 1.5f;
    }
}
