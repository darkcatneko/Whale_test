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
        //���UBoss
        //Debug.Log(Controller.ThisStage.ThisRoundBoss);
        Controller.M_BossController.SetBossData(Controller.ThisStage.ThisRoundBoss);//
        //���U�Z�� 
        InstallWeapon();
        //���U�D�Ԫ�
        InstallMainFighter();
        //�ǳƶ}�l
        //�i�J
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
            Controller.WeaponButton[i].GetComponent<WeaponButtonAction>().ButtonWeapon = new WeaponPackClass(Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]), Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]).Weapon_BreakLevel);
        }
        for (int i = 0; i < Controller.m_MainPlayer.BringingWeaponID.Length; i++)
        {
            Controller.m_MainPlayer.AddWeaponMath(Controller.W_Data.GetWeaponInformation(Controller.m_MainPlayer.BringingWeaponID[i]));
        }
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
