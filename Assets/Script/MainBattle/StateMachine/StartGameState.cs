using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameState : Istate
{
    public GameController Controller { get; set; }
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
        Controller.GameMap.SpawnMap();
        Controller.GameMap.SpawnMapObject(Controller.GameMap.ThisMap);
        //�ǳƶ}�l
        //�i�J
    }
    public void OnStateStay()
    {
        Controller.ChangeState(StateEnum.Free_State); 

    }
    public void OnStateExit()
    {

    }
}
