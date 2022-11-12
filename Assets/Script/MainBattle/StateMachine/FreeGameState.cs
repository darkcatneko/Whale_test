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
        Debug.Log("�i�ۥ�");
        Controller = controller;
    }
    public void OnStateStay()
    {
        if (Input.touchCount>0)
        {
            if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
            {
                Controller.ChangeState(StateEnum.Hold_State);
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            Controller.ChangeState(StateEnum.Hold_State);
        }
    }
    public void OnStateExit()
    {

    }
}
