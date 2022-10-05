using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    public int[] WeaponPool = new int[10];
    public int[] BringingWeaponID = new int[5];
    //public WeaponPackClass[] WeaponPack = new WeaponPackClass[5];

    

    public int Attack;
    public int Armor;
    public int Heal;
    public int Crit;

    private void Start()
    {
    }
}
