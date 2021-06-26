using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidScript : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("walk", !animator.GetBool("walk"));
        }
        
        if (Input.GetKeyDown(KeyCode.Return) && animator.GetBool("walk"))
        {
            animator.SetBool("run", !animator.GetBool("run"));
        }

        if (animator.GetBool("walk"))
        {
            var p = transform.position;
            p += transform.forward * 0.01f;
            transform.position = p;
            if (animator.GetBool("run"))
            {
                p = transform.position;
                p += transform.forward * 0.03f;
                transform.position = p;
            }
        }
    }
}

