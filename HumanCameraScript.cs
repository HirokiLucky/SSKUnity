using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class HumanCameraScript : MonoBehaviour
{
    public GameObject target;
    public float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null){return;}

        var p = target.transform.position;
        transform.position = p + target.transform.forward * distance * -1 + Vector3.up;
        p.y = 1f;
        transform.LookAt(p);
    }
}
