using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class FreeGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        //�i�H�}�l�ާ@
        Controller = controller;
    }
    public void OnStateStay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Controller.ChangeState(StateEnum.Hold_State);
        }
        
    }
    public void OnStateExit()
    {

    }
}
