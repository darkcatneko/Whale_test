using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainCharacterData", menuName = "MainCharacterData")]

public class MainCharacterData : ScriptableObject
{
    public List<MainCharacterSO> MainCharacterDataList;
    public MainCharacterSO GetCharacterInformation(int _id)
    {
        for (int i = 0; i < MainCharacterDataList.Count; i++)
        {
            if (MainCharacterDataList[i].ID == _id)
            {
                return MainCharacterDataList[i];
            }
        }
        return null;
    }
}

