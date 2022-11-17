using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class WeaponButtonAction : MonoBehaviour
{
    public WeaponPackClass ButtonWeapon;
    public int NowCoolDown = 0;
    public GameController GM;
    private Vector2 LastFocus = new Vector2 (10,10);
    private Camera MainCam;
    private Dictionary<int, UnityAction> WeaponSkillData;
    private UnityEvent WeaponSkill = new UnityEvent();
    public void ButtonAction()
    {
        //Debug.Log(ButtonWeapon.m_WeaponID.ToString());
    }
    private void Start()
    {
        MainCam = Camera.main;
        WeaponSkillData = new Dictionary<int, UnityAction>
        {
            {0,OnR_SlashWeaponPointerUp},
            //{1,  },
            //{2,  },
            //{3,  }
        };
    }
    public void OnPointerDown()
    {
        GM.ChangeState(StateEnum.Skill_State);
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
        if (GM.GetState() == StateEnum.Skill_State)
        {
            GM.ChangeState(StateEnum.Free_State);
        }
    }
    public void InstallWeaponSkill()
    {
        if (WeaponSkillData.ContainsKey(ButtonWeapon.m_WeaponID))
        {
            WeaponSkill.AddListener(WeaponSkillData[ButtonWeapon.m_WeaponID]);
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
            if (GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockType == WeaponEnum.Slash&& GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockLevel>0)
            {
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.BuffAmount = 0.2f;
                GM.GameMap.FindBlock(ButtonWeapon.m_RuneHoverPoints[i] + Origin).ThisBlockBuff.BuffRound = 1;   
            }          
        }
        GM.GameMap.RefreshMap();
        NowCoolDown = 0;
        GM.m_MainPlayer.SkillActivation++;
        GM.ChangeState(StateEnum.Free_State);
    }
    public void OnR_SlashWeaponPointerUp()
    {

        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//璶эtouch
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (NowCoolDown == ButtonWeapon.m_WeaponCD)
            {
                if (hit.transform.tag == "Block")
                {
                    StartCoroutine("R_SlashWeapon_Func", new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
                }
                else if (LastFocus != new Vector2(10, 10))
                {
                    StartCoroutine("R_SlashWeapon_Func", LastFocus);
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
                StartCoroutine("R_SlashWeapon_Func", LastFocus);
            }
            else
            {
                GM.ChangeState(StateEnum.Free_State);
            }
        }
        LastFocus = new Vector2(10, 10);

        //GM.GameMap.TextTest(GM.GameMap.ThisMap);


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
