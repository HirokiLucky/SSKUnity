using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherData : MonoBehaviour
{
    private Dictionary<string, int> param = new Dictionary<string, int>()
    {
        {"power", 1},
        {"level", 1},
        {"exp", 0},
    };

    Color[] levelColor =
    {
        new Color(0f, 0f, 0.75f),
        new Color(0f, 0f, 1f),
        new Color(0.25f, 0.25f, 1f),
        new Color(0.5f, 0.5f, 1f),
        new Color(0.75f, 0.75f, 1f),
        new Color(1f, 1f, 1f),
    };

    float dv = 1f;

    GameObject player = null;
    HelloWorld sss = null;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sss = player.GetComponent<HelloWorld>();
    }

    public int Level()
    {
        return param["level"];
    }

    public void AddExp(int n)
    {
        param["exp"] += n;
        if (param["exp"] >= 10)
        {
            levelUp();
        }
    }

    void levelUp()
    {
        if (param["level"] == 5)
        {
            return;
        }

        param["level"]++;
        param["exp"] = 0;
        var r = gameObject.GetComponent<Renderer>();
        r.material.color = levelColor[param["level"]];
        dv = param["level"];
    }

    void Fight()
    {
        var pr = player.GetComponent<Rigidbody>();
        var mr = GetComponent<Rigidbody>();
        if (pr.velocity.magnitude > mr.velocity.magnitude)
        {
            Debug.Log(gameObject.name + ": Lose...");
            param["power"] -= sss.Level();
            AddExp(sss.Level()/2);
            if (param["power"] <= 0)
            {
                sss.Flag(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log(gameObject.name + ": Gotcha!");
            AddExp(sss.Level());
        }
    }

    void FixedUpdate()
    {
        if (sss.Finish())
        {
            return;
        }

        var dp = player.transform.position - transform.position;
        var r = GetComponent<Rigidbody>();
        r.AddForce(dp/10 * dv);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sss.Finish())
        {
            return;
        }

        if (collision.gameObject.name == "Player")
        {
            var halo = (Behaviour) GetComponent("Halo");
            halo.enabled = true;
            Fight();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(sss.Finish()){return;}
        if(collision.gameObject.name == "Player")
        {
            var halo = (Behaviour) GetComponent("Halo");
            halo.enabled = false;
        }
    }
}
