using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonAction : MonoBehaviour
{
    public WeaponPackClass ButtonWeapon;
    public GameController GM;
    private Vector2 LastFocus = new Vector2 (10,10);
    private Camera MainCam;
    public void ButtonAction()
    {
        //Debug.Log(ButtonWeapon.m_WeaponID.ToString());
    }
    private void Start()
    {
        MainCam = Camera.main;
    }
    public void OnPointerDown()
    {
        GM.ChangeState(StateEnum.Hold_State);
    }
    public void Ondrag()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.touches[0].position);
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
    public void OnPointerUp()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.touches[0].position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Block")
            {
                //Debug.Log(hit.transform.GetComponent<BlockIdentity>().ThisColumn + "    " + hit.transform.GetComponent<BlockIdentity>().ThisRow);
                //代刚才ゅ
                GM.GameMap.MixBlock2(ButtonWeapon.m_RuneHoverPoints.ToArray(), new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
            }
            else if (LastFocus != new Vector2(10, 10))
            {
                GM.GameMap.MixBlock2(ButtonWeapon.m_RuneHoverPoints.ToArray(), LastFocus);
            }
        }
        else if (LastFocus!= new Vector2(10,10))
        {
            GM.GameMap.MixBlock2(ButtonWeapon.m_RuneHoverPoints.ToArray(), LastFocus);
        }
        LastFocus = new Vector2(10, 10);

        GM.GameMap.TextTest(GM.GameMap.ThisMap);

        GM.ChangeState(StateEnum.Free_State);
    }
   
}
