using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "NewWeapon")]
public class WeaponSO : ScriptableObject
{
    public bool Unlock;
    public int Owned;
    public int Weapon_BreakLevel;
    public int WeaponID;
    public string Weapon_Name;
    public WeaponEnum WeaponType;
    public int[] Durability = new int[5];
    public int[] Atk = new int[5];
    public int[] Armor = new int[5];
    public int[] Heal = new int[5];
    public int[] Crit = new int[5];
    public List<Vector2> RuneHoverPoints = new List<Vector2>();

    //BasicSkill
    //MainSkill
    public int MainSkillCD;
    //Story

    public void Install(bool savebool, int saveown,int savebreak)
    {
        Unlock = savebool;
        Owned = saveown;
        Weapon_BreakLevel = savebreak;
    }
}

