using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
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
                    }
                    else
                    {
                        Controller.M_BossController.Be_Attack(Controller.m_MainPlayer.Attack, (int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType, Controller.m_MainPlayer.Buff_Amount);
                    }                   
                }
            }
        }
        Controller.ChangeState(StateEnum.Defence_State);
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
}
