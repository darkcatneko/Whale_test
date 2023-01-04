using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : Istate
{
    public GameController Controller { get; set; }

   
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        Controller.playerDataSystem.Save();
        Controller.EndGame(0);
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
}
