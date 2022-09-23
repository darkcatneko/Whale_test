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
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Block")
                {
                    Debug.Log(hit.transform.GetComponent<BlockIdentity>().ThisColumn + "    " + hit.transform.GetComponent<BlockIdentity>().ThisRow);
                    //´ú¸Õ²Å¤å
                    Controller.GameMap.BlockInRange(Controller.Test_1.RuneHoverPoints.ToArray(), new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow));
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Controller.ChangeState(StateEnum.Free_State);
        }
    }
    public void OnStateExit()
    {

    }
}
