using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSkill : MonoBehaviour
{
    public GameController GM;
    private Vector2 LastFocus = new Vector2(10, 10);
    private Camera MainCam;
    public List<Vector2> RuneHoverPoints = new List<Vector2>();
    private void Start()
    {
        MainCam = Camera.main;
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
                GM.GameMap.BlockInRange(RuneHoverPoints.ToArray(), new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
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
                    StartCoroutine("SlashMainCharacterFunc");
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
                StartCoroutine("SlashMainCharacterFunc");
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
            GM.GameMap.SpawnSingleMapObject(WeaponEnum.Slash, 2, (int)(RuneHoverPoints[i].y + Origin.y), (int)(RuneHoverPoints[i].x + Origin.x), GM.GameMap.StartAmmo);
            GM.GameMap.FindBlock(RuneHoverPoints[i] + Origin).ThisBlockType = WeaponEnum.Slash;
        }
        GM.m_MainPlayer.SkillActivation++;
        GM.ChangeState(StateEnum.Free_State);
    }
}
