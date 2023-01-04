using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class PlayerDataSystem : MonoBehaviour
{
    [SerializeField] PlayerData ThisPlayer;
    [SerializeField] MainPlayer M_MainPlayer;
    [SerializeField] WeaponData W_Data;
    private void Awake()
    {
        
    }
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckNewData()
    {
        if (ThisPlayer.ThisAccount.NewAccount == true)
        {
            ThisPlayer.ThisAccount.LastLoginYear = DateTime.Now.Year;
            ThisPlayer.ThisAccount.LastLoginMonth = DateTime.Now.Month;
            ThisPlayer.ThisAccount.LastLoginDay = DateTime.Now.Day;
            ThisPlayer.ThisAccount.CoinCount = 10000;
            ThisPlayer.ThisAccount.GemCount = 15000;
            for (int i = 0; i < 15; i++)
            {
                ThisPlayer.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot = false;
            }
            for (int i = 0; i < 5; i++)
            {
                ThisPlayer.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot = true;
            }
            for (int i = 0; i < ThisPlayer.ThisAccount.HaveCharactorOrNot.Count; i++)
            {
                ThisPlayer.ThisAccount.HaveCharactorOrNot[i] = false;
            }
            ThisPlayer.ThisAccount.HaveCharactorOrNot[1] = true;
            ThisPlayer.ThisAccount.NowMainCharactor = 1;
            ThisPlayer.ThisAccount.NewAccount = false;
            Save();
            Load();
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Save.ept", FileMode.Create);
        bf.Serialize(stream, ThisPlayer.ThisAccount);
        stream.Close();
        Debug.Log("Save Complete" + Application.dataPath + "/Save.ept");
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.ept"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Save.ept", FileMode.Open);
            DeepCopy(ThisPlayer, bf.Deserialize(stream) as PlayerAccount);
            stream.Close();
            Debug.Log("Load");
            for (int i = 0; i <ThisPlayer.ThisAccount.WeaponBackpack.Length; i++)
            {
                M_MainPlayer.BringingWeaponID[i] = ThisPlayer.ThisAccount.WeaponBackpack[i];
            }
            M_MainPlayer.ThisRound_MainCharacter_ID = ThisPlayer.ThisAccount.NowMainCharactor;
            for (int i = 0; i < W_Data.WeaponDataList.Count; i++)
            {
                W_Data.WeaponDataList[i].Install(ThisPlayer.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot, ThisPlayer.ThisAccount.M_WeaponSaveFile[i].Owned, ThisPlayer.ThisAccount.M_WeaponSaveFile[i].WeaponBreakLevel);
            }
        }
        else
        {
            CheckNewData();
        }
    }
    public void LoginEvent()
    {

    }
    public void DeepCopy(PlayerData A, PlayerAccount B)
    {
        A.ThisAccount.CoinCount = B.CoinCount;
        A.ThisAccount.GemCount = B.GemCount;
        A.ThisAccount.NewAccount = B.NewAccount;
        A.ThisAccount.LastLoginYear = B.LastLoginYear;
        A.ThisAccount.LastLoginMonth = B.LastLoginMonth;
        A.ThisAccount.LastLoginDay = B.LastLoginDay;
        for (int i = 0; i < A.ThisAccount.M_WeaponSaveFile.Count; i++)
        {
            A.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot = B.M_WeaponSaveFile[i].HaveWeaponOrNot;
            A.ThisAccount.M_WeaponSaveFile[i].Owned = B.M_WeaponSaveFile[i].Owned;
            A.ThisAccount.M_WeaponSaveFile[i].WeaponBreakLevel = B.M_WeaponSaveFile[i].WeaponBreakLevel;
        }
        for (int i = 0; i < A.ThisAccount.HaveCharactorOrNot.Count; i++)
        {
            A.ThisAccount.HaveCharactorOrNot[i] = B.HaveCharactorOrNot[i];
        }
        for (int i = 0; i < A.ThisAccount.WeaponBackpack.Length; i++)
        {
            A.ThisAccount.WeaponBackpack[i] = B.WeaponBackpack[i];
        }
        A.ThisAccount.NowMainCharactor = B.NowMainCharactor;
    }
    public void IntoBattle()
    {
        M_MainPlayer.ThisRound_MainCharacter_ID = ThisPlayer.ThisAccount.NowMainCharactor;
        for (int i = 0; i < ThisPlayer.ThisAccount.WeaponBackpack.Length; i++)
        {
            M_MainPlayer.BringingWeaponID[i] = ThisPlayer.ThisAccount.WeaponBackpack[i];
        }
        Save();
        SceneManager.LoadScene(1);
    }
    private void OnApplicationQuit()
    {
        ThisPlayer.ThisAccount.LastLoginYear =0;
        ThisPlayer.ThisAccount.LastLoginMonth = 0;
        ThisPlayer.ThisAccount.LastLoginDay = 0;
        ThisPlayer.ThisAccount.CoinCount = 10000;
        ThisPlayer.ThisAccount.GemCount = 15000;        
        for (int i = 0; i < 15; i++)
        {
            ThisPlayer.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot = false;
        }
        for (int i = 0; i < 5; i++)
        {
            ThisPlayer.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot = true;
        }
        for (int i = 0; i < ThisPlayer.ThisAccount.HaveCharactorOrNot.Count; i++)
        {
            ThisPlayer.ThisAccount.HaveCharactorOrNot[i] = false;
        }
        ThisPlayer.ThisAccount.HaveCharactorOrNot[1] = true;
        ThisPlayer.ThisAccount.NowMainCharactor = 1;
        ThisPlayer.ThisAccount.NewAccount = true;
    }
}
