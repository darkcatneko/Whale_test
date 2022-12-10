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
        //註冊Boss
        //Debug.Log(Controller.ThisStage.ThisRoundBoss);
        Controller.M_BossController.SetBossData(Controller.ThisStage.ThisRoundBoss);//
        //註冊武器 
        InstallWeapon();
        //註冊主戰者
        InstallMainFighter();
        //準備開始
        //進入
    }
    public void OnStateStay()
    {
        Controller.ChangeState(StateEnum.Ready_State); 

    }
    public void OnStateExit()
    {
        
    }
    public void InstallWeapon()
    {        
        for (int i = 0; i < 5; i++)
        {
            if (Controller.m_MainPlayer.BringingWeaponID[i]!=999)
            {
                Controller.WeaponButton[i].GetComponent<WeaponButtonAction>().ButtonWeapon = new WeaponPackClass(Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]), Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]).Weapon_BreakLevel);
            }           
        }
        for (int i = 0; i < Controller.m_MainPlayer.BringingWeaponID.Length; i++)
        {
            if (Controller.m_MainPlayer.BringingWeaponID[i] != 999)
            {
                Controller.m_MainPlayer.AddWeaponMath(Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]));
                Controller.WeaponButton[i].GetComponent<WeaponButtonAction>().InstallWeaponSkill();
            }           
        }
        ///
        bool[] Checker = new bool[Controller.W_Data.WeaponDataList.Count];
        for (int i = 0; i < Controller.m_MainPlayer.BringingWeaponID.Length; i++)
        {
            if (Controller.m_MainPlayer.BringingWeaponID[i]!=999)
            {
                Checker[Controller.m_MainPlayer.BringingWeaponID[i]] = true;
            }   
        }
        if (Checker[4])
        {
            float buffamount = 0;
            switch (Controller.W_Data.WeaponDataList[4].Weapon_BreakLevel+1)
            {
                case 1:
                    buffamount = 0.02f;
                    break;
                case 2:
                    buffamount = 0.025f;
                    break;
                case 3:
                    buffamount = 0.03f;
                    break;
                case 4:
                    buffamount = 0.035f;
                    break;
                case 5:
                    buffamount = 0.04f;
                    break;
            }
            Controller.m_MainPlayer.Regen_Buff_Amount += buffamount;
        }
        if (Checker[14])
        {
            float buffamount = 0;
            switch (Controller.W_Data.WeaponDataList[14].Weapon_BreakLevel+1)
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
            Controller.m_MainPlayer.Regen_Buff_Amount += buffamount;
        }
        ///
        Controller.m_MainPlayer.NowArmor = Controller.m_MainPlayer.MaxArmor;
    }
    public void InstallMainFighter()
    {
        Controller.m_MainPlayer.ThisRoundCharacter = Controller.C_Data.GetCharacterInformation(Controller.m_MainPlayer.ThisRound_MainCharacter_ID);
        Controller.MaxMP = Controller.m_MainPlayer.ThisRoundCharacter.SkillUseMP;
        for (int i = 0; i < Controller.C_Data.GetCharacterInformation(Controller.m_MainPlayer.ThisRound_MainCharacter_ID).RuneHoverPoints.Count; i++)
        {
            Controller.MainCharacterSkillButton.GetComponent<MainCharacterSkill>().RuneHoverPoints.Add(Controller.C_Data.GetCharacterInformation(Controller.m_MainPlayer.ThisRound_MainCharacter_ID).RuneHoverPoints[i]);
        }
        Controller.MainCharacterSkillButton.GetComponent<MainCharacterSkill>().InstallSkill();
    }
}
