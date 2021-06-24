using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System.Timers;
using UnityEngine.UI;

public class HelloWorld : MonoBehaviour
{
    Dictionary<string, bool> flag = new Dictionary<string, bool>()
    {
        {"Sphere 0", false},
        {"Sphere 1", false},
        {"Sphere 2", false},
        {"Sphere 3", false},
    };

    Dictionary<string, int> param = new Dictionary<string, int>()
    {
        {"power", 1},
        {"level", 1},
        {"exp", 0},
    };

    // Start is called before the first frame update
    bool finish = false;
    Vector3 cv = new Vector3(0f, 3f, -10f);
    Rigidbody rb = null;
    GameObject ex = null;
    Text message = null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        message = GameObject.Find("Message").GetComponent<Text>();
        ex = GameObject.Find("BigExplosion");
    }

    void FixedUpdate()
    {
        if (Finish())
        {
            return;
        }

        var sv = transform.position;
        sv.y = 1f;
        Camera.main.transform.position = sv + cv;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var v = new Vector3(x * param["level"] * 10, 0, y * param["level"] * 10);
        rb.AddForce(v);
    }

    public int Level()
    {
        return param["level"];
    }

    public bool Finish()
    {
        return finish;
    }

    public void AddExp(int n)
    {
        param["exp"] += n;
        if (param["exp"] >= 10)
        {
            levelUp();
        }
    }

    void Fight(GameObject go)
    {
        var er = go.GetComponent<Rigidbody>();
        var pr = GetComponent<Rigidbody>();
        var od = er.GetComponent<OtherData>();
        if (er.velocity.magnitude > pr.velocity.magnitude)
        {
            Debug.Log("Player: Loss...");
            param["power"] -= od.Level();
            AddExp(od.Level()/2);
            if (param["power"] <= 0)
            {
                Loss();
            }
        }
        else
        {
            Debug.Log("Player: Gotcha!");
            ex.transform.position = go.transform.position;
            ex.GetComponent<ParticleSystem>().Play();
            AddExp(od.Level());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Finish())
        {
            return;
        }
        if (collision.gameObject.tag == "Other")
        {
            var data = collision.gameObject.GetComponent<OtherData>();
            Fight(collision.gameObject);
        }
    }

    public void Flag(string flg)
    {
        flag[flg] = true;
        if (CheckFlag())
        {
            Win();
        }
    }

    bool CheckFlag()
    {
        var f = true;
        foreach (var item in flag)
        {
            if (item.Value == false)
            {
                f = false;
            }
        }
        return f;
    }
    
    void levelUp()
    {
        if (param["level"] == 5)
        {
            return;
        }
        param["level"]++;
        param["exp"] = 0;
        message.text = "Level " + param["level"];
        TimerStart();
    }

    void TimerStart()
    {
        StartCoroutine(TimerEnd());
    }

    IEnumerator TimerEnd()
    {
        yield return new WaitForSeconds(3f);
        message.text = "";
    }

    void Loss()
    {
        message.text = "Game Over";
        finish = true;
    }

    void Win()
    {
        message.text = "Win!!";
        finish = true;
    }
    
}
