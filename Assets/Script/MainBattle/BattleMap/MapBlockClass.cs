using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapBlockClass 

{
    public WeaponEnum ThisBlockType;
    public float ThisBlockEnergy;
    public float ThisBlockLevel { get { return MathF.Log(ThisBlockEnergy, 2); } }
    public GameObject m_ThisBlockObject;
    public MapBlockClass(WeaponEnum TBT,float TBE)
    {
        ThisBlockType = TBT;
        ThisBlockEnergy = TBE;
    }
    public void SetRandomMapBlock()
    {
        ThisBlockType = (WeaponEnum)UnityEngine.Random.Range(0, 5);
        ThisBlockEnergy = 1;
    }
}

