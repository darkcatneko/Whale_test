using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : Istate
{
    public GameController Controller { get; set; }

   
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;

    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
}
