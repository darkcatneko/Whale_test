using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        //��������CD
        Controller.M_BossController.CD_To_Next_Attack--;
        if (Controller.M_BossController.CD_To_Next_Attack ==0)
        {
            Debug.Log("BossAttack");
            Controller.M_BossController.BossNormalAttack();
        }
        Controller.ChangeState(StateEnum.Ready_State);
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
}
