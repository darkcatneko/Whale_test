using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        Controller.GameMap.SpawnMap();
        Controller.GameMap.SpawnMapObject(Controller.GameMap.ThisMap);
        //註冊武器 
        InstallWeapon();
        //準備開始
        //進入
    }
    public void OnStateStay()
    {
        Controller.ChangeState(StateEnum.Free_State); 

    }
    public void OnStateExit()
    {

    }
    public void InstallWeapon()
    {
        
        for (int i = 0; i < 5; i++)
        {
            Controller.WeaponButton[i].GetComponent<WeaponButtonAction>().ButtonWeapon = new WeaponPackClass(Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]), Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]).Weapon_BreakLevel);
        }
    }
}
