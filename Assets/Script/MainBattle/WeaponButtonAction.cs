using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class WeaponButtonAction : MonoBehaviour
{
    public bool SkillActivating;
    public WeaponPackClass ButtonWeapon;
    public int NowCoolDown = 0;
    public GameController GM;
    private Vector2 LastFocus = new Vector2 (10,10);
    private Camera MainCam;
    private Dictionary<int, Action> WeaponSkillData;
    private UnityEvent WeaponSkill = new UnityEvent();
    public void ButtonAction()
    {
        //Debug.Log(ButtonWeapon.m_WeaponID.ToString());
    }
    private void Start()
    {
        MainCam = Camera.main;
        WeaponSkillData = new Dictionary<int, Action>
        {
            { 0,()=>{BasicPointerUpFunc("R_SlashWeapon_Func"); } },
            {1, ()=>{BasicPointerUpFunc("R_SpearWeapon_Func"); } },
            {2, ()=>{BasicPointerUpFunc("R_HammerWeapon_Func"); }},
            {3,  ()=>{BasicPointerUpFunc("R_BowWeapon_Func"); }},
            {4,  ()=>{BasicPointerUpFunc("R_ShieldWeapon_Func"); }},
            {10,  ()=>{BasicPointerUpFunc("SSR_SlashWeapon_Func"); }},
            {11,  ()=>{BasicPointerUpFunc("SSR_SakuraDreamWeapon_Func"); }},
            {12,  ()=>{BasicPointerUpFunc("SSR_PunishmentWeapon_Func"); }},
            {13,  ()=>{BasicPointerUpFunc("SSR_RequiemWeapon_Func"); }},
            {14,  ()=>{BasicPointerUpFunc("SSR_GoldenTimeWeapon_Func"); }},
            {5,  ()=>{BasicPointerUpFunc("SR_knightSwordWeapon_Func"); }},
            {6,  ()=>{BasicPointerUpFunc("SR_knightSpearWeapon_Func"); }},
            {7,  ()=>{BasicPointerUpFunc("SR_knightHammerWeapon_Func"); }},
            //{8,  ()=>{BasicPointerUpFunc("SR_knightSwordWeapon_Func"); }},
            //{9,  ()=>{BasicPointerUpFunc("SR_knightSwordWeapon_Func"); }}
        };
    }
    public void OnPointerDown()
    {
        if (GM.GetState() == StateEnum.Free_State)
        {
            GM.ChangeState(StateEnum.Skill_State);
        }
    }
    public void OnHover()
    {
        if (GM.GetState()==StateEnum.Free_State)
        {
            GM.ChangeState(StateEnum.Skill_State);           
        }       
    }
    public void OnHoverExit()
    {
        if (GM.GetState() == StateEnum.Skill_State&&SkillActivating == false)
        {
            GM.ChangeState(StateEnum.Free_State);
        }
    }
    public void InstallWeaponSkill()
    {
        if (WeaponSkillData.ContainsKey(ButtonWeapon.m_WeaponID))
        {
            WeaponSkill.AddListener(WeaponSkillData[ButtonWeapon.m_WeaponID].Invoke);
        }
    }
    public void WeaponSkillButton()
    {
        WeaponSkill.Invoke();
    }
    public void Ondrag()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//璶эtouch
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Block")
            {
                //Debug.Log(hit.transform.GetComponent<BlockIdentity>().ThisColumn + "    " + hit.transform.GetComponent<BlockIdentity>().ThisRow);
                LastFocus = new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow);
                //代刚才ゅ
                GM.GameMap.BlockInRange(ButtonWeapon.m_RuneHoverPoints.ToArray(), new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
            }
        }
    }
    #region R_weapon
    public IEnumerator R_SlashWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType!=WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Slash && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.2f));
                }
            }                     
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[0] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator R_SpearWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Lunge && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(WeaponEnum.Lunge, (int)GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel + 1, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo, 0, 0, 0);
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Lunge;
                }
            }            
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[1] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator R_HammerWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType!= WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Hit && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.2f));
                }
            }           
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[2] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator R_BowWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Penetrate && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(WeaponEnum.Penetrate, (int)GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel + 1, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo, 0, 0, 0);
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Penetrate;
                }
            }
            
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[3] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator R_ShieldWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Penetrate && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(WeaponEnum.Penetrate, 1, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo, 0, 0, 0);
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Armor;
                }
            }
            
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[4] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public void BasicPointerUpFunc(string FuncName)
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//璶эtouch
        RaycastHit hit;
        if (SkillActivating == false)
        {
            //Debug.Log(0);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(1);
                if (NowCoolDown == ButtonWeapon.m_WeaponCD)
                {
                    //Debug.Log(2);
                    if (hit.transform.tag == "Block")
                    {
                        //Debug.Log(3);
                        GM.ChangeState(StateEnum.Animation_State);
                        SkillActivating = true;
                        StartCoroutine(FuncName, new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
                    }
                    else if (LastFocus != new Vector2(10, 10))
                    {
                        GM.ChangeState(StateEnum.Animation_State);
                        SkillActivating = true;
                        StartCoroutine(FuncName, LastFocus);
                    }
                    else
                    {
                        GM.ChangeState(StateEnum.Free_State);
                    }
                }
                else
                {
                    GM.ChangeState(StateEnum.Free_State);
                }
            }
            else if (LastFocus != new Vector2(10, 10))
            {
                if (NowCoolDown == ButtonWeapon.m_WeaponCD)
                {
                    GM.ChangeState(StateEnum.Animation_State);
                    SkillActivating = true;
                    StartCoroutine(FuncName, LastFocus);
                }
                else
                {
                    GM.ChangeState(StateEnum.Free_State);
                }
            }
            else if (LastFocus == new Vector2(10, 10)&& SkillActivating == false)
            {
                GM.ChangeState(StateEnum.Free_State);
            }           
        }
        
        LastFocus = new Vector2(10, 10);

        //GM.GameMap.TextTest(GM.GameMap.ThisMap);
    }
    #endregion
    #region SSR_weapon
    public IEnumerator SSR_SlashWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Slash && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(WeaponEnum.Slash, 4, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), 2, 0, 0, 0);
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Slash;
                }
                else if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.Slash && GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType, 0, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), 0, 0, 0, 0);
                }
            }           
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[10] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SSR_SakuraDreamWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Lunge, 1, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), 1, 0, 0, 0);
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Slash;
            }
                           
        }
        for (int i = 0; i < GM.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < GM.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.3f));
            }
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[11] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SSR_PunishmentWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ShieldLeft += 1;
            }           
        }
        List<Vector2> Temp = new List<Vector2>();
        for (int i = 0; i < GM.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < GM.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (GM.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == 0&& GM.GameMap.ThisMap[i].ThisRow[j].ThisBlockType == WeaponEnum.Hit)
                {
                    Temp.Add(new Vector2(j, i));
                }
            }
        }
        if (Temp.Count>3)
        {
            int[] RandomNum = new int[3];
            for (int i = 0; i < 3; i++)
            {
                RandomNum[i] = UnityEngine.Random.Range(0, Temp.Count);
                for (int j = 0; j < i; j++)
                {
                    while(RandomNum[i] == RandomNum[j])
                    {
                        RandomNum[i] = UnityEngine.Random.Range(0, Temp.Count);
                    }
                }
            }
            for (int i = 0; i < RandomNum.Length; i++)
            {                 
                Destroy(GM.GameMap.FindBlock(Temp[RandomNum[i]]).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Hit, 1, (int)Temp[RandomNum[i]].y, (int)Temp[RandomNum[i]].x, GM.GameMap.StartAmmo, 0, 0, 0);
                GM.GameMap.FindBlock(Temp[RandomNum[i]]).ThisBlockType = WeaponEnum.Hit;
            }
        }
        else
        {
            for (int i = 0; i < Temp.Count; i++)
            {
                Destroy(GM.GameMap.FindBlock(Temp[i]).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Hit, 1, (int)Temp[i].y, (int)Temp[i].x, GM.GameMap.StartAmmo, 0, 0, 0);
                GM.GameMap.FindBlock(Temp[i]).ThisBlockType = WeaponEnum.Hit;
            }
        }
        
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[12] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SSR_RequiemWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Penetrate, 1, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), 1, 0, 0, 0);
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Penetrate;
            }            
        }
        for (int i = 0; i < GM.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < GM.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (GM.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0 && GM.GameMap.ThisMap[i].ThisRow[j].ThisBlockType == WeaponEnum.Penetrate)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(WeaponEnum.Penetrate, (int)Mathf.Clamp(GM.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel + 1,0,5) , (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), 1, 0, 0, 0);
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Penetrate;
                }
            }
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[13] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SSR_GoldenTimeWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType!=WeaponEnum.None)
            {
                Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Armor, 2, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo, 0, 0, 0);
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Armor;
            }           
        }
        GM.GameMap.RefreshMap();
        ///
        GM.WeaponSkillActivation[14] = true;
        float buffamount = 0;
        switch (GM.W_Data.WeaponDataList[14].Weapon_BreakLevel+1)
        {
            case 1:
                buffamount = 0.06f;
                break;
            case 2:
                buffamount = 0.075f;
                break;
            case 3:
                buffamount = 0.09f;
                break;
            case 4:
                buffamount = 0.105f;
                break;
            case 5:
                buffamount = 0.12f;
                break;
        }
        GM.m_MainPlayer.Regen_Buff_Amount += buffamount;
        Debug.LogWarning("腳旅狦秨﹍" + GM.m_MainPlayer.Regen_Buff_Amount.ToString());
        ///
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SR_knightSwordWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType!=WeaponEnum.None)
            {
                Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Slash, 1, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), 1, 0, 0, 0);
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Slash;
            }
            
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[5] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SR_knightSpearWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                    GM.GameMap.SpawnSingleMapObject(WeaponEnum.Lunge, (int)GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo, 0, 0, 0);
                    GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Lunge;
                }
            }
                    
        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[6] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SR_knightHammerWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Hit)
                    {
                        GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.3f));
                    }
                    else
                    {
                        GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.15f));
                    }
                }
            }

        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[7] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SR_knightBowWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Penetrate)
                    {
                        GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1,0f,"KnightBow"));
                    }                    
                }
            }

        }
        GM.GameMap.RefreshMap();
        GM.WeaponSkillActivation[8] = true;
        NowCoolDown = 0;
        SkillActivating = false;
        GM.ChangeState(StateEnum.Free_State);
    }
    public IEnumerator SR_knightShieldWeapon_Func(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ButtonWeapon.m_RuneHoverPoints.Count; i++)
        {
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType != WeaponEnum.None)
            {
                if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel > 0)
                {
                    if (ButtonWeapon.m_RuneHoverPoints[i] == Vector2.zero)
                    {
                        Destroy(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                        GM.GameMap.SpawnSingleMapObject(WeaponEnum.Armor, 2, (int)(ButtonWeapon.m_RuneHoverPoints[i].y + Origin.y), (int)(ButtonWeapon.m_RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo, 0, 0, 0);
                        GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Armor;
                        GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.25f));
                    }
                    else if(GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Armor)
                    {
                        GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.Add(new BlockBuff(1, 0.25f));
                    }
                    
                }
            }

        }
    }
    #endregion
    public void OnPointerUp()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//璶эtouch
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Block")
            {
                //Debug.Log(hit.transform.GetComponent<BlockIdentity>().ThisColumn + "    " + hit.transform.GetComponent<BlockIdentity>().ThisRow);
                //代刚才ゅ
                //GM.GameMap.MixBlock2(ButtonWeapon.m_RuneHoverPoints.ToArray(), new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
            }
            else if (LastFocus != new Vector2(10, 10))
            {
               // GM.GameMap.MixBlock2(ButtonWeapon.m_RuneHoverPoints.ToArray(), LastFocus);
            }
        }
        else if (LastFocus!= new Vector2(10,10))
        {
           // GM.GameMap.MixBlock2(ButtonWeapon.m_RuneHoverPoints.ToArray(), LastFocus);
        }
        LastFocus = new Vector2(10, 10);

        //GM.GameMap.TextTest(GM.GameMap.ThisMap);

        GM.ChangeState(StateEnum.Free_State);
    }
    
}
