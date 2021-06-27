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
        var b = animator.GetFloat("Blend");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("walk", !animator.GetBool("walk"));
        }

        if (animator.GetBool("walk"))
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                b += 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                b -= 0.1f;
            }
            animator.SetFloat("Blend", b);
        }

        if (animator.GetBool("walk"))
        {
            var p = transform.position;
            var d = 1.0f + b * 3;
            p += transform.forward * (d / 100);
            transform.position = p;
        }
    }
}

