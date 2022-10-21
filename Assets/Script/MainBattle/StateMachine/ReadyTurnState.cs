using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyTurnState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        Debug.Log("¶i·Ç³Æ");
        Controller.ReadyTurnFunc();
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
    
}
