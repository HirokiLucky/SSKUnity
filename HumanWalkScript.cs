using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWalkScript : MonoBehaviour
{
    GUIStyle panelStyle;
    GUIStyle labelStyle;

    bool flag = false;
    string message;

    Animator animator;

    public void SetFlag(bool f)
    {
        flag = f;
    }

    public void SetMessage(string s)
    {
        message = s;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        message = "start.";
        animator = GetComponent<Animator>();
    }

    void GuiSetUp()
    {
        panelStyle = new GUIStyle(GUI.skin.box);
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if (!flag)
        {
            return;
        }
        GuiSetUp();
        GUILayout.BeginArea(new Rect(10, 10, 310, 210));
        GUILayout.BeginVertical(panelStyle);
        GUILayout.Box("Information");
        GUILayout.Label(message, labelStyle);
        GUILayout.EndArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag)
        {
            return;
        }
        animator.SetFloat("Speed", 0f);
        animator.SetFloat("Direction", 0f);
        animator.SetBool("Jump", false);
        animator.SetBool("Rest", false);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetFloat("Speed", 1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetFloat("Speed", -0.5f);
        }

        var s = animator.GetFloat("Speed");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("Speed", s/3);
        }

        s = animator.GetFloat("Speed");
        transform.position += transform.forward * 0.02f * s;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            var d = animator.GetFloat("Direction");
            animator.SetFloat("Direction", 0.2f);
            animator.SetFloat("GravityControl", 0.2f);
            transform.Rotate(0f, 0.1f, 0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            var d = animator.GetFloat("Direction");
            animator.SetFloat("Direction", -0.2f);
            animator.SetFloat("GravityControl", 0.2f);
            transform.Rotate(0f, -0.1f, 0f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!flag){return;}

        if (collision.gameObject.tag == "Player")
        {
            message = "collision Other Player.";
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(!flag){return;}

        if (collision.gameObject.tag == "Player")
        {
            message = "collision exit";
        }
    }
}
