using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public PlayerAccount ThisAccount;    
    
}
[System.Serializable]
public class PlayerAccount
{
    public bool NewAccount = true;
    public int LastLoginYear;
    public int LastLoginMonth;
    public int LastLoginDay;
    public int CoinCount;
    public int GemCount;
    public List<WeaponSaveFile> M_WeaponSaveFile = new List<WeaponSaveFile>();
    public List<bool> HaveCharactorOrNot = new List<bool>();
    public int[] WeaponBackpack = new int[5] { 0, 1, 2, 3, 4 };
    public int NowMainCharactor = 1;
    public int EntranceTime = 3;

}
[System.Serializable]
public class WeaponSaveFile
{
    public bool HaveWeaponOrNot = false;
    public int Owned = 0;
    public int WeaponBreakLevel = 0;
}

