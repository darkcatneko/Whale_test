using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapBlockClass 

{
    public WeaponEnum ThisBlockType;
    public float ThisBlockLevel;
    public int AmmoLeft;
    public int ShieldLeft;
    public GameObject m_ThisBlockObject;
    public MapBlockClass(WeaponEnum TBT,float TBE,int AL,int SL)
    {
        ThisBlockType = TBT;
        ThisBlockLevel = TBE;
        AmmoLeft = AL;
        ShieldLeft = SL;
    }
    public void SetRandomMapBlock()
    {
        ThisBlockType = (WeaponEnum)UnityEngine.Random.Range(0, 5);
        ThisBlockLevel = 0;
    }
    
}

