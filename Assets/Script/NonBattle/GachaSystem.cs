using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    [SerializeField] PlayerData Data;
    [SerializeField] WeaponData weaponData;
    public void Gacha()
    {
        int random = Random.Range(0, 100);
        if (random>96)//3%
        {
            int randomWeapon = 
        }

    }
}
