using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class GachaSystem : MonoBehaviour
{
    [SerializeField] PlayerDataSystem DataSystem;
    [SerializeField] PlayerData Data;
    [SerializeField] WeaponData weaponData;
    [SerializeField] MainCharacterData mc_data;
    public VideoPlayer VD;
    public RenderTexture RD;
    public VideoClip[] VC = new VideoClip[2];
    public GameObject GachaPrefab;
    public GameObject Canvas;
    public void Gacha()
    {
        int random = Random.Range(0, 100);
        Data.ThisAccount.GemCount -= 150;
        if (random > 96)//3%
        {
            int randomCha = Random.Range(0, 4);
            GachaVideo(true, mc_data.MainCharacterDataList[randomCha].GachaImage);
            if (!GetCha(randomCha))
            {
                Data.ThisAccount.CoinCount += 1000;
            }
            Debug.Log(randomCha + "¨¤¦â");
        }
        else if (random > 90)
        {
            int randomWeapon = Random.Range(10, 15);
            GachaVideo(true, weaponData.WeaponDataList[randomWeapon].GachaImage);
            if (!GetWeapon(randomWeapon))
            {
                Data.ThisAccount.CoinCount += 500;
            }

            Debug.Log(randomWeapon);
        }
        else if (random > 70)
        {
            int randomWeapon = Random.Range(5, 10);
            GachaVideo(false, weaponData.WeaponDataList[randomWeapon].GachaImage);
            if (!GetWeapon(randomWeapon))
            {
                Data.ThisAccount.CoinCount += 100;
            }
            Debug.Log(randomWeapon);
        }
        else
        {
            int randomWeapon = Random.Range(0, 5);
            GachaVideo(false, weaponData.WeaponDataList[randomWeapon].GachaImage);
            if (!GetWeapon(randomWeapon))
            {
                Data.ThisAccount.CoinCount += 50;
            }
            Debug.Log(randomWeapon);
        }
        DataSystem.Save();
    }
    public void GachaVideo(bool SSR, Sprite Image)
    {
        if (SSR)
        {
            VD.clip = VC[0];
            ClearOutRenderTexture(RD);
            GameObject OBJ = Instantiate(GachaPrefab, Canvas.transform);

            VD.Play();
            OBJ.GetComponentsInChildren<Image>()[1].sprite = Image;
        }
        else
        {
            VD.clip = VC[1];
            ClearOutRenderTexture(RD);
            GameObject OBJ = Instantiate(GachaPrefab, Canvas.transform);

            VD.Play();
            OBJ.GetComponentsInChildren<Image>()[1].sprite = Image;
        }

    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
    public bool GetWeapon(int weaponID)
    {
        if (Data.ThisAccount.M_WeaponSaveFile[weaponID].HaveWeaponOrNot & Data.ThisAccount.M_WeaponSaveFile[weaponID].Owned == 5)
        {
            return false;
        }
        Data.ThisAccount.M_WeaponSaveFile[weaponID].HaveWeaponOrNot = true;
        Data.ThisAccount.M_WeaponSaveFile[weaponID].Owned = Mathf.Clamp(Data.ThisAccount.M_WeaponSaveFile[weaponID].Owned + 1, 0, 5);
        weaponData.WeaponDataList[weaponID].Unlock = true;
        weaponData.WeaponDataList[weaponID].Owned = Mathf.Clamp(Data.ThisAccount.M_WeaponSaveFile[weaponID].Owned + 1, 0, 5);
        return true;
    }
    public bool GetCha(int ChaID)
    {
        if (Data.ThisAccount.HaveCharactorOrNot[ChaID] == true)
        {
            return false;
        }
        Data.ThisAccount.HaveCharactorOrNot[ChaID] = true;
        return true;
    }
}
