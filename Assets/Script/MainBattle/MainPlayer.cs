using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MainPlayerSO", menuName = "MainPlayer")]
public class MainPlayer : ScriptableObject
{
    public int[] WeaponPool = new int[10];
    public int[] BringingWeaponID = new int[5];
    public int ThisRound_MainCharacter_ID = 0;
    public WeaponPackClass[] WeaponPack = new WeaponPackClass[5];
    public MainCharacterSO ThisRoundCharacter;

    public int SkillActivation = 0;

    public int Attack;
    public int MaxArmor;
    public int NowArmor;
    public int Heal;
    public int Crit;
    public float Buff_Amount;
    public float Regen_Buff_Amount;
    public void AddWeaponMath(WeaponSO thisweapon)
    {
        Attack += thisweapon.Atk[thisweapon.Weapon_BreakLevel];
        MaxArmor += thisweapon.Armor[thisweapon.Weapon_BreakLevel];
        Heal += thisweapon.Heal[thisweapon.Weapon_BreakLevel];
        Crit += thisweapon.Crit[thisweapon.Weapon_BreakLevel];
    }
    public void Regenerate(int level)
    {
        float Regen = 0;
        float LevelReg = 0;
        switch (level)
        {
            case 1:
                LevelReg = 1f;
                break;
            case 2:
                LevelReg = 2.5f;
                break;
            case 3:
                LevelReg = 5f;
                break;
            case 4:
                LevelReg = 8.75f;
                break;
            case 5:
                LevelReg = 13f;
                break;
        }
        Regen = Heal * LevelReg * Regen_Buff_Amount;
        Regen = Mathf.RoundToInt(Regen);
        Debug.LogWarning("我回了" + Regen + "點");
        NowArmor = (int)Mathf.Clamp(Regen + NowArmor, 0, MaxArmor);
    }
    public void Reset()
    {
        Attack = 0;
        MaxArmor = 0;
        NowArmor = 0;
        Heal = 0;
        Crit = 0;
    }
}
