using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

enum Attack
{
    None,
    Punch,
    Kick
}

public class FightScript : MonoBehaviour
{
    public FightData fdata;
    Text message;
    GameObject enemy;
    Animator animator;
    float wd = 0.01f;
    Attack attack = Attack.None;
    Dictionary<string, KeyCode> kmap;
    
    // Start is called before the first frame update
    void Start()
    {
        fdata.Init();
        animator = GetComponent<Animator>();
        message = GameObject.Find("Text").GetComponent<Text>();
        var list = GameObject.FindGameObjectsWithTag("Player");
        if (list[0] == gameObject)
        {
            enemy = list[1];
        }
        else
        {
            enemy = list[0];
        }

        if (gameObject.name == "unitychan")
        {
            kmap = fdata.GetKokuseiMap();
        }
        else
        {
            kmap = fdata.GetBoxMap();
        }

        StartCoroutine(TimerEnd());
    }

    IEnumerator TimerEnd()
    {
        yield return new WaitForSeconds(3f);
        message.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (fdata.IsEnd())
        {
            return;
        }

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
        
        transform.LookAt(enemy.transform);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (fdata.IsEnd())
        {
            return;
        }

        if (collider.tag != "Player")
        {
            return;
        }

        if (name == "unitychan")
        {
            if (attack == Attack.Punch)
            {
                var halo = (Behaviour) GameObject.Find("kokusei punch").GetComponent("Halo");
                halo.enabled = true;
                fdata.kokuseiHitPunch();
            }
            if (attack == Attack.Kick)
            {
                var halo = (Behaviour) GameObject.Find("kokusei kick").GetComponent("Halo");
                halo.enabled = true;
                fdata.kokuseiHitKick();
            }
        }
        
        if (name == "BoxUnityChan")
        {
            if (attack == Attack.Punch)
            {
                var halo = (Behaviour) GameObject.Find("box punch").GetComponent("Halo");
                halo.enabled = true;
                fdata.boxHitPunch();
            }
            if (attack == Attack.Kick)
            {
                var halo = (Behaviour) GameObject.Find("box kick").GetComponent("Halo");
                halo.enabled = true;
                fdata.boxHitKick();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        AllOffHalo();
        if (fdata.IsEnd())
        {
            Finish();
        }
    }

    void AllOffHalo()
    {
        var halo = (Behaviour) GameObject.Find("kokusei punch").GetComponent("Halo");
        halo.enabled = false;
        halo = (Behaviour) GameObject.Find("kokusei kick").GetComponent("Halo");
        halo.enabled = false;
        halo = (Behaviour) GameObject.Find("box punch").GetComponent("Halo");
        halo.enabled = false;
        halo = (Behaviour) GameObject.Find("box kick").GetComponent("Halo");
        halo.enabled = false;
    }

    void Finish()
    {
        if (fdata.Win() == name)
        {
            animator.SetBool("win", true);
            Debug.Log(fdata.Win() + "の体力は" + fdata.GetKokusei() );
        }
        else
        {
            animator.SetBool("loss", true);
        }
        message.text = fdata.Win() + "Win!!";
    }
}
