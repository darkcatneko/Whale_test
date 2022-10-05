using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
    }
    public void OnStateStay()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Controller.ChangeState(StateEnum.Free_State);
        //}
    }
    public void OnStateExit()
    {

    }
}
