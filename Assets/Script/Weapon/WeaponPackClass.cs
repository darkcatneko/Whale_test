using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponPackClass 
{
    public WeaponEnum m_WeaponType;
    public int m_WeaponID;
    public int m_Durability;
    public int m_WeaponCD;
    public List<Vector2> m_RuneHoverPoints = new List<Vector2>();
    public WeaponPackClass(WeaponSO Data,int WeaponLevel)
    {
        m_WeaponType = Data.WeaponType;
        m_WeaponID = Data.WeaponID;
        m_Durability = Data.Durability[WeaponLevel];
        m_WeaponCD = Data.MainSkillCD;
        for (int i = 0; i < Data.RuneHoverPoints.Count; i++)
        {
            m_RuneHoverPoints.Add(Data.RuneHoverPoints[i]);
        }
    }
}
