using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "fight1", menuName = "ScriptableObjects/fight", order = 1)]

public class fight : ScriptableObject
{
    int p1Power = 100;
    int p2Power = 100;

    public void Init()
    {
        p1Power = 100;
        p2Power = 100;
    }

    public void p1Attack()
    {
        p2Power -= 10;
    }

    public void p2Attack()
    {
        p1Power -= 10;
    }

    public int getp1()
    {
        return p1Power;
    }

    public int getp2()
    {
        return p2Power;
    }

    public bool IsEnd()
    {
        return p1Power <= 0 || p2Power <= 0;
    }

    public string Win()
    {
        if (p1Power <= 0 && p2Power > 0)
        {
            return "PLAYER 1 IS WINNER !!";
        }
        if (p2Power <= 0 && p1Power > 0)
        {
            return "PLAYER 2 IS WINNER !!";
        }
        return null;
    }
}
