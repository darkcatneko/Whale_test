using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldGameState : Istate
{
    public GameController Controller { get; set; }
    private int FocusCount = 0;
    private Vector2[] TwoFocusBlock = new Vector2[2];
    public void OnStateEnter(GameController controller)
    {
        Controller = controller;
    }
    public void OnStateStay()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    Ray ray = Controller.MainCam.ScreenPointToRay(Input.touches[0].position);
                    
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "Block")
                        {
                            Controller.GameMap.BlockImFocus(new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow),FocusCount,TwoFocusBlock);
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    Ray ray1 = Controller.MainCam.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit hit1;
                    if (Physics.Raycast(ray1, out hit1))
                    {
                        if (hit1.transform.tag == "Block")
                        {
                            switch (FocusCount)
                            {
                                case 0:
                                    Controller.GameMap.FingerLifted(new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow));
                                    TwoFocusBlock[FocusCount] = new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow);
                                    FocusCount++;
                                    return;
                                case 1:
                                    if (new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow) != TwoFocusBlock[0])
                                    {
                                        Controller.GameMap.FingerLifted(new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow));
                                        TwoFocusBlock[FocusCount] = new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow);
                                        FocusCount++;
                                        if (FocusCount == 2)
                                        {
                                            Debug.Log("twoBlock!!");
                                            Controller.GameMap.MixTwoBlock(TwoFocusBlock[0], TwoFocusBlock[1]);
                                            FocusCount = 0;
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("SameBlock!!");
                                    }
                                    return;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            TwoFocusBlock = new Vector2[2];
                            FocusCount = 0;
                            Controller.GameMap.RefreshMap();
                        }
                    }
                    else
                    {
                        TwoFocusBlock = new Vector2[2];
                        FocusCount = 0;
                        Controller.GameMap.RefreshMap();
                    }
                    break;
                default:
                    break;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Controller.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Block")
                {
                    Controller.GameMap.BlockImFocus(new Vector2(hit.transform.GetComponent<BlockIdentity>().ThisColumn, hit.transform.GetComponent<BlockIdentity>().ThisRow), FocusCount, TwoFocusBlock);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray1 = Controller.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1))
            {
                if (hit1.transform.tag == "Block")
                {
                    switch (FocusCount)
                    {
                        case 0:
                            Controller.GameMap.FingerLifted(new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow));
                            TwoFocusBlock[FocusCount] = new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow);
                            FocusCount++;
                            Controller.ChangeState(StateEnum.Free_State);
                            return;
                        case 1:
                            if (new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow) != TwoFocusBlock[0])
                            {
                                Controller.GameMap.FingerLifted(new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow));
                                TwoFocusBlock[FocusCount] = new Vector2(hit1.transform.GetComponent<BlockIdentity>().ThisColumn, hit1.transform.GetComponent<BlockIdentity>().ThisRow);
                                FocusCount++;
                                if (FocusCount == 2)
                                {
                                    //Debug.Log("twoBlock!!");
                                    if (Controller.GameMap.MixTwoBlock(TwoFocusBlock[0], TwoFocusBlock[1]))
                                    {
                                        Controller.TurnPoint--;      
                                        //操作動數-1                
                                        Controller.NowMP = Mathf.Clamp((int)(Controller.NowMP + Controller.GameMap.FindBlock(TwoFocusBlock[1]).ThisBlockLevel), 0, (int)Controller.MaxMP);
                                        //增加量條
                                        if (Controller.TurnPoint <= 0)
                                        {
                                            Controller.ChangeState(StateEnum.Attack_State);
                                        }
                                    }                           
                                    FocusCount = 0;
                                    Controller.ChangeState(StateEnum.Free_State);
                                }
                                Controller.ChangeState(StateEnum.Free_State);
                            }
                            else
                            {
                                //Debug.Log("SameBlock!!");
                            }
                            return;
                        default:
                            break;
                    }
                }
                else
                {
                    TwoFocusBlock = new Vector2[2];
                    FocusCount = 0;
                    Controller.GameMap.RefreshMap();
                }
            }
            else
            {
                TwoFocusBlock = new Vector2[2];
                FocusCount = 0;
                Controller.GameMap.RefreshMap();
            }
        }
    }
    public void OnStateExit()
    {

    }
}
