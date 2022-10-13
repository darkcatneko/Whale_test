using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyTurnState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        Controller.TurnPoint = Controller.GameMap.TurnPointGain(3);//¥ý°òÂ¦3
        controller.ChangeState(StateEnum.Free_State);
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
}
