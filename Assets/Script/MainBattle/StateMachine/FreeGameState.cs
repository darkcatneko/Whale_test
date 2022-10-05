using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class FreeGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        //可以開始操作
        Controller = controller;
    }
    public void OnStateStay()
    {
            
    }
    public void OnStateExit()
    {

    }
}
