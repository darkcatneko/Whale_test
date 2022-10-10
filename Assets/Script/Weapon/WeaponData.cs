using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    public List<WeaponSO> WeaponDataList;
    public WeaponSO GetWeaponInformation(int _id)
    {
        for (int i = 0; i < WeaponDataList.Count; i++)
        {
            if (WeaponDataList[i].WeaponID == _id)
            {
                return WeaponDataList[i];
            }
        }
        return null;
    }
}
