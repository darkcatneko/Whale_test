using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
public class PlayerDataSystem : MonoBehaviour
{
    [SerializeField] PlayerData ThisPlayer;
    [SerializeField] MainPlayer M_MainPlayer;
    [SerializeField] WeaponData W_Data;
    private void Awake()
    {
        Load();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckNewData()
    {
        if (ThisPlayer.NewAccount == true)
        {
            ThisPlayer.LastLoginYear = DateTime.Now.Year;
            ThisPlayer.LastLoginMonth = DateTime.Now.Month;
            ThisPlayer.LastLoginYear = DateTime.Now.Day;
            for (int i = 0; i < 5; i++)
            {
                ThisPlayer.M_WeaponSaveFile[i].HaveWeaponOrNot = true;
            }
            ThisPlayer.HaveCharactorOrNot[1] = true;
            ThisPlayer.NowMainCharactor = 1;
            ThisPlayer.NewAccount = true;
            Save();
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Save.ept", FileMode.Create);
        bf.Serialize(stream, ThisPlayer);
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
            DeepCopy(ThisPlayer, bf.Deserialize(stream) as PlayerData);
            stream.Close();
            Debug.Log("Load");
            for (int i = 0; i <ThisPlayer.WeaponBackpack.Length; i++)
            {
                M_MainPlayer.BringingWeaponID[i] = ThisPlayer.WeaponBackpack[i];
            }
            M_MainPlayer.ThisRound_MainCharacter_ID = ThisPlayer.NowMainCharactor;
            for (int i = 0; i < W_Data.WeaponDataList.Count; i++)
            {
                //W_Data.WeaponDataList[i].Install(ThisPlayer.M_WeaponSaveFile[i].HaveWeaponOrNot)

            }
        }
        else
        {
            CheckNewData();
        }
    }

    public void DeepCopy(PlayerData A, PlayerData B)
    {
        A.NewAccount = B.NewAccount;
        A.LastLoginYear = B.LastLoginYear;
        A.LastLoginMonth = B.LastLoginMonth;
        A.LastLoginDay = B.LastLoginDay;
        for (int i = 0; i < A.M_WeaponSaveFile.Count; i++)
        {
            A.M_WeaponSaveFile[i].HaveWeaponOrNot = B.M_WeaponSaveFile[i].HaveWeaponOrNot;
            A.M_WeaponSaveFile[i].Owned = B.M_WeaponSaveFile[i].Owned;
            A.M_WeaponSaveFile[i].WeaponBreakLevel = B.M_WeaponSaveFile[i].WeaponBreakLevel;
        }
        for (int i = 0; i < A.HaveCharactorOrNot.Count; i++)
        {
            A.HaveCharactorOrNot[i] = B.HaveCharactorOrNot[i];
        }
        for (int i = 0; i < A.WeaponBackpack.Length; i++)
        {
            A.WeaponBackpack[i] = B.WeaponBackpack[i];
        }
        A.NowMainCharactor = B.NowMainCharactor;
    }
}
