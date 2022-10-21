using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class MainCharacterSkill : MonoBehaviour
{
    public GameController GM;
    private Vector2 LastFocus = new Vector2(10, 10);
    private Camera MainCam;
    public List<Vector2> RuneHoverPoints = new List<Vector2>();
    private Dictionary<int, UnityAction> MainCharacterSkillData;
    private UnityEvent SkillEvent = new UnityEvent();
    private void Start()
    {
        MainCam = Camera.main;
        MainCharacterSkillData = new Dictionary<int, UnityAction>
        {
            {0, OnSlashMainCharacterPointerUp},
            {1, OnIIresPointerUp }
        };
               
    }
    public void OnPointerDown()
    {
        GM.ChangeState(StateEnum.Skill_State);
    }
    public void OnHover()
    {
        if (GM.GetState() == StateEnum.Free_State)
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
    public void InstallSkill()
    {
        if (MainCharacterSkillData.ContainsKey(GM.m_MainPlayer.ThisRoundCharacter.ID))
        {
            SkillEvent.AddListener(MainCharacterSkillData[GM.m_MainPlayer.ThisRoundCharacter.ID]);
        }
    }
    public void SkillButton()
    {
        SkillEvent.Invoke();
    }
    public void RefreshListener()
    {
        SkillEvent.RemoveAllListeners();
    }
    public void Ondrag()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//要改回touch
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Block"&&GM.NowMP == GM.MaxMP)
            {
                //Debug.Log(hit.transform.GetComponent<BlockIdentity>().ThisColumn + "    " + hit.transform.GetComponent<BlockIdentity>().ThisRow);
                LastFocus = new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow);
                //測試符文
                if (RuneHoverPoints.Count>0)
                {
                    GM.GameMap.BlockInRange(RuneHoverPoints.ToArray(), new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
                }                
            }
        }
    }
    
    public void OnSlashMainCharacterPointerUp()
    {
        
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//要改回touch
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (GM.NowMP == GM.MaxMP)
            {
                if (hit.transform.tag == "Block")
                {
                    StartCoroutine("SlashMainCharacterFunc", new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));                  
                }
                else if (LastFocus != new Vector2(10, 10))
                {
                    StartCoroutine("SlashMainCharacterFunc",LastFocus);
                }
            }
            else
            {
                GM.ChangeState(StateEnum.Free_State);
            }
        }
        else if (LastFocus != new Vector2(10, 10))
        {
            if (GM.NowMP == GM.MaxMP)
            {
                StartCoroutine("SlashMainCharacterFunc", LastFocus);
            }
            else
            {
                GM.ChangeState(StateEnum.Free_State);
            }
        }
        LastFocus = new Vector2(10, 10);

        //GM.GameMap.TextTest(GM.GameMap.ThisMap);
       

    }
    public IEnumerator SlashMainCharacterFunc(Vector2 Origin)
    {
        for (int i = 0; i < GM.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < GM.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (GM.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    GM.GameMap.DestroyAndRefreshSingleBlock(new Vector2(j, i));
                }
            }
        }
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < RuneHoverPoints.Count; i++)
        {
            Destroy(GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).m_ThisBlockObject);
            GM.GameMap.SpawnSingleMapObject(WeaponEnum.Slash, 2, (int)(RuneHoverPoints[i].y + Origin.y), (int)(RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo,0);
            GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Slash;
        }
        GM.m_MainPlayer.SkillActivation++;
        GM.ChangeState(StateEnum.Free_State);
    }
    public void OnIIresPointerUp()
    {
       
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);//要改回touch
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (GM.NowMP == GM.MaxMP)
            {
                if (hit.transform.tag == "Block")
                {
                    StartCoroutine("HitMainCharacterFunc", new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
                }
                else if (LastFocus != new Vector2(10, 10))
                {
                    StartCoroutine("HitMainCharacterFunc", LastFocus);
                }
            }
            else
            {
                GM.ChangeState(StateEnum.Free_State);
            }
        }
        else if (LastFocus != new Vector2(10, 10))
        {
            if (GM.NowMP == GM.MaxMP)
            {
                StartCoroutine("HitMainCharacterFunc", LastFocus);
            }
            else
            {
                GM.ChangeState(StateEnum.Free_State);
            }
        }
        LastFocus = new Vector2(10, 10);

        //GM.GameMap.TextTest(GM.GameMap.ThisMap);


    }
    public IEnumerator HitMainCharacterFunc(Vector2 Origin)
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < RuneHoverPoints.Count; i++)
        {
            if (RuneHoverPoints[i] == new Vector2(0,0))
            {
                Destroy(GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Hit, 3, (int)(RuneHoverPoints[i].y + Origin.y), (int)(RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo,0);
                GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Hit;
            }
            else
            {
                Destroy(GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).m_ThisBlockObject);
                GM.GameMap.SpawnSingleMapObject(WeaponEnum.Hit, 1, (int)(RuneHoverPoints[i].y + Origin.y), (int)(RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo,0);
                GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Hit;
            }            
        }
        GM.m_MainPlayer.SkillActivation++;
        GM.ChangeState(StateEnum.Free_State);
    }
    private void OnApplicationQuit()
    {
        RefreshListener();
    }
}
