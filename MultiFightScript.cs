using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

    public class MultiFightScript : MonoBehaviour
{
    MultiFightPun2Script pun2Script;
    Animator animator;
    bool flag = false;
    float wd = 0.01f;
    Attack attack = Attack.None;
    Dictionary<string, KeyCode> kmap = new Dictionary<string, KeyCode>()
    {
        {"forward", KeyCode.I},
        {"back", KeyCode.K},
        {"right", KeyCode.L},
        {"left", KeyCode.J},
        {"punch", KeyCode.U},
        {"kick", KeyCode.O}
    };

    public void SetFlag(bool f)
    {
        Debug.Log("SetFlag.");
        flag = f;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        pun2Script = GameObject.Find("Pun2Object").GetComponent<MultiFightPun2Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!flag){return;}
        if(!pun2Script.IsFlag()){return;}

        if (Input.GetKeyDown(kmap["forward"]))
        {
            animator.SetBool("walk", true);
            wd = 0.01f;
        }
        if (Input.GetKeyUp(kmap["forward"]))
        {
            animator.SetBool("walk", false);
        }
        
        if (Input.GetKeyDown(kmap["back"]))
        {
            animator.SetBool("walk", true);
            wd = -0.01f;
        }
        if (Input.GetKeyUp(kmap["back"]))
        {
            animator.SetBool("walk", false);
        }
        
        if (Input.GetKeyDown(kmap["right"]))
        {
            transform.position += transform.right * 0.1f;
        }

        if (Input.GetKeyDown(kmap["left"]))
        {
            transform.position -= transform.right * 0.1f;
        }

        if (animator.GetBool("walk"))
        {
            var p = transform.position;
            p += transform.forward * wd;
            transform.position = p;
        }

        if (Input.GetKeyDown(kmap["punch"]))
        {
            animator.SetBool("punch", true);
            attack = Attack.Punch;
        }

        if (Input.GetKeyDown(kmap["kick"]))
        {
            var r = animator.transform.rotation;
            animator.transform.rotation = r;
            animator.SetBool("kick", true);
            attack = Attack.Kick;
        }

        if (pun2Script.GetEnemy() == null)
        {
            pun2Script.CheckEnemy();
        }
        else
        {
            transform.LookAt(pun2Script.GetEnemy().transform);        
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(!flag){return;}
        if(collider.tag != "Player"){return;}

        if (attack == Attack.Punch)
        {
            var halo = (Behaviour) GameObject.Find("Punch").gameObject.GetComponent("Halo");
            halo.enabled = true;
            var prps = pun2Script.GetEnemyPlayer().CustomProperties;
            var n = prps["power"] is int value ? value : 0;
            prps["power"] = n - 5;
            pun2Script.GetEnemyPlayer().SetCustomProperties(prps);
        }
        if (attack == Attack.Kick)
        {
            var halo = (Behaviour) GameObject.Find("Kick").gameObject.GetComponent("Halo");
            halo.enabled = true;
            var prps = pun2Script.GetEnemyPlayer().CustomProperties;
            var n = prps["power"] is int value ? value : 0;
            prps["power"] = n - 7;
            pun2Script.GetEnemyPlayer().SetCustomProperties(prps);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        AllOffHalo();
    }

    void AllOffHalo()
    {
        foreach (var ob in GameObject.FindGameObjectsWithTag("Collider"))
        {
            var halo = (Behaviour) ob.GetComponent("Halo");
            halo.enabled = false;
        }
    }
}
