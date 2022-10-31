using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameState : Istate
{
    public GameController Controller { get; set; }
    private int[] TowerCount = new int[5];
    public void OnStateEnter(GameController controller)
    {
        Debug.Log("¶i§ðÀ»");
        Controller = controller;
        for (int i = 0; i < Controller.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < Controller.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType == WeaponEnum.Armor)
                    {
                        Controller.m_MainPlayer.Regenerate((int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel);
                        if (Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft>0)
                        {
                            Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft--;
                        }                       
                        TowerCount[0]++;
                    }
                    else
                    {
                        Controller.M_BossController.Be_Attack(Controller.m_MainPlayer.Attack, (int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType, Controller.m_MainPlayer.Buff_Amount);
                        CharacterPassiveEnergyCharge();
                        if (Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft > 0)
                        {
                            Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft--;
                        }
                        TowerCount[(int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType]++;
                    }                   
                }
            }
        }
        BossSpecialCheck();
        TowerCount = new int[5];
        Controller.ChangeState(StateEnum.Defence_State);
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
    public void BossSpecialCheck()
    {
        if (BossSpecialMoveDestroy(Boss.WormBo) && Controller.M_BossController.GetBoss() == Boss.WormBo)
        {
            Controller.M_BossController.BossSpecialAttack();
        }
        if (Controller.M_BossController.GetBoss() == Boss.MainBoss && Controller.M_BossController.NowSkillName == "SpecialAttackB")
        {
            for (int i = 1; i < TowerCount.Length; i++)
            {
                if (TowerCount[i] > 0)
                {
                    Controller.M_BossController.BossSpecialAttack();
                }
            }
        }
        if (Controller.M_BossController.GetBoss() == Boss.MainBoss && Controller.M_BossController.NowSkillName == "SpecialAttackE")
        {
            for (int i = 1; i < TowerCount.Length; i++)
            {
                if (TowerCount[i] > 0)
                {
                    Controller.M_BossController.BossSpecialAttack();
                }
            }
        }
    }
    public void CharacterPassiveEnergyCharge()
    {
        switch(Controller.m_MainPlayer.ThisRound_MainCharacter_ID)
        {
            case 3:
                Controller.NowMP = Mathf.Clamp(Controller.NowMP+1,0, Controller.MaxMP);
                return;
        }
    }
    public bool BossSpecialMoveDestroy(Boss boss)
    {
        bool complete = false;
        switch (boss)
        {
            case Boss.WormBo:                
                int max = 0;
                for (int i = 0; i < TowerCount.Length; i++)
                {
                    if (TowerCount[i]>0)
                    {
                        max++;
                    }
                }
                if (max>=3)
                {
                    complete = true;                    
                }
                return complete;          
        }
        Debug.Log("bruh");
        return complete;
    }
    
}
