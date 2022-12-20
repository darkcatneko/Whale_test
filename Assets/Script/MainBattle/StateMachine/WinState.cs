using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        Controller.M_BossController.BossAnimation(0, 3);
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
}
