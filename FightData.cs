using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "fight_data", menuName = "ScriptableObjects/FightData", order = 1)]
public class FightData : ScriptableObject
{
    Dictionary<string, KeyCode> kokusei_map = new Dictionary<string, KeyCode>()
    {
        {"forward", KeyCode.W},
        {"back", KeyCode.S},
        {"right", KeyCode.D},
        {"left", KeyCode.A},
        {"punch", KeyCode.E},
        {"kick", KeyCode.Q}
    };
    
    Dictionary<string, KeyCode> box_map = new Dictionary<string, KeyCode>()
    {
        {"forward", KeyCode.I},
        {"back", KeyCode.K},
        {"right", KeyCode.K},
        {"left", KeyCode.J},
        {"punch", KeyCode.O},
        {"kick", KeyCode.U}
    };

    int kokusei_power = 100;
    int box_power = 100;
    // Start is called before the first frame update
    public void Init()
    {
        kokusei_power = 100;
        box_power = 100;
    }

    public Dictionary<string, KeyCode> GetKokuseiMap()
    {
        return kokusei_map;
    }
    
    public Dictionary<string, KeyCode> GetBoxMap()
    {
        return box_map;
    }

    public void kokuseiHitPunch()
    {
        box_power -= 5;
    }
    
    public void kokuseiHitKick()
    {
        box_power -= 7;
    }
    
    public void boxHitPunch()
    {
        kokusei_power -= 5;
    }
    
    public void boxHitKick()
    {
        kokusei_power -= 7;
    }

    public int GetKokusei()
    {
        return kokusei_power;
    }
    
    public int GetBox()
    {
        return box_power;
    }

    public bool IsEnd()
    {
        return kokusei_power <= 0 || box_power <= 0;
    }

    public string Win()
    {
        if (kokusei_power <= 0 && box_power > 0)
        {
            return "BoxUnityChan";
        }
        if (kokusei_power > 0 && box_power <= 0) ;
        {
            return "unitychan";
        }
        return null;
    }
}
