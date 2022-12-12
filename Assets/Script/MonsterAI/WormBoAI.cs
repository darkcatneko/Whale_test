using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WormBoAI : BossIstate
{
    //�Ĥ@��Boss
    public BossController Controller { get; set; }
    public bool InRage = false;
    public bool SpecialMove1 = false; public bool SpecialMove2 = false; public bool SpecialMove3 = false;
    public bool SpecialMove3Unmove = false;
    public UnityEvent BossAction = new UnityEvent();
    public void OnRoundEnter(BossController controller)
    {
        Controller = controller;
        Controller.Set_Stat(1, 50000f, 800f, 700f, 0.8f, 1f, 1.2f, 0.9f, 1);
    }
    public void ResetRound()
    {
        Controller.CD_To_Next_Attack = 1;
    }
    #region BossChoose�S��
    public void BossChooseSpecialMoveA()
    {
        Debug.Log("�S����A");
        SpecialMove1 = true;
        List<Vector2> temp = new List<Vector2>();
        for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    temp.Add(new Vector2(j, i));
                }
            }
        }
        if (temp.Count > 0)
        {
            int spot = Random.Range(0, temp.Count);
            for (int i = 0; i < 5; i++)
            {
                Controller.GenWarning(new Vector2(temp[spot].x, i));
                Controller.BlockReadyToBreak.Add(new Vector2(temp[spot].x, i));
                Controller.GameMaster.GameMap.FindBlock(new Vector2(temp[spot].x, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
            }
        }
        BossAction.AddListener(() =>
        {
            Controller.BossAnimation(0,1);
            Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
            Controller.BossAttackDamage(2f);
            if (Controller.BlockReadyToBreak.Count > 0)
            {
                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                {
                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 1));                    
                }
            }
            
            Controller.BlockReadyToBreak = new List<Vector2>();
            BossAction.RemoveAllListeners();
        });
    }
    public void BossChooseSpecialMoveB()
    {
        Debug.Log("�S����B");
        SpecialMove2 = true;
        List<Vector2> temp = new List<Vector2>();
        for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    temp.Add(new Vector2(j, i));
                }
            }
        }
        if (temp.Count > 0)
        {
            int spot = Random.Range(0, temp.Count);
            for (int i = 0; i < 5; i++)
            {
                if (new Vector2(temp[spot].x, i) != temp[spot])
                {
                    Controller.GenWarning(new Vector2(temp[spot].x, i));                   
                    Controller.BlockReadyToBreak.Add(new Vector2(temp[spot].x, i));                   
                    Controller.GameMaster.GameMap.FindBlock(new Vector2(temp[spot].x, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                   
                }
                if (new Vector2(i, temp[spot].y) != temp[spot])
                {
                    Controller.GenWarning(new Vector2(i, temp[spot].y));
                    Controller.BlockReadyToBreak.Add(new Vector2(i, temp[spot].y));
                    Controller.GameMaster.GameMap.FindBlock(new Vector2(i, temp[spot].y)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
            }
            Controller.GenWarning(temp[spot]);
            Controller.BlockReadyToBreak.Add(temp[spot]);
            Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
        }
        BossAction.AddListener(() =>
        {
            Controller.BossAnimation(0, 1);
            Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
            Controller.BossAttackDamage(1.2f);
            if (Controller.BlockReadyToBreak.Count > 0)
            {
                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                {
                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 1));
                }
            }
            
            Controller.BlockReadyToBreak = new List<Vector2>();
            BossAction.RemoveAllListeners();
        });
    }
    public void BossChooseSpecialMoveC()
    {
        Debug.Log("��������");
        SpecialMove3 = true;
        BossAction.AddListener(() =>
        {
            if (SpecialMove3Unmove == false)
            {
                Controller.BossAnimation(0, 0);
                Debug.Log("�����j����");
                Controller.BossAttackDamage(3f);
            }
            else
            {
                Controller.BossAnimation(0, 0);
                Debug.Log("�����p����");
                Controller.BossAttackDamage(1f);
            }
            Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);

            Controller.BlockReadyToBreak = new List<Vector2>();
            BossAction.RemoveAllListeners();
        });
    }
    #endregion
    public void BossChooseAttack()
    {
        if (Controller.NowHealth <= Controller.MaxHealth *0.7f && SpecialMove1 == false&&Controller.NowHealth > Controller.MaxHealth * 0.4f )
        {
            BossChooseSpecialMoveA();
        }
        else
        {
            if (Controller.NowHealth <= Controller.MaxHealth * 0.4f && SpecialMove2 == false && Controller.NowHealth > Controller.MaxHealth * 0.1f)
            {
                BossChooseSpecialMoveB();
            }
            else
            {
                if (Controller.NowHealth <= Controller.MaxHealth * 0.1f && SpecialMove3 == false)
                {
                    BossChooseSpecialMoveC();
                }
                else
                {
                    if (InRage == false)
                    {
                        int a = Random.Range(0, 100);
                        if (a <= 30)
                        {
                            Debug.Log("����A�w��");
                            BossAction.AddListener(() => { Controller.BossAnimation(0, 0);  Controller.AttackUsedTime += Controller.BossAnimationTime(0, 0); Controller.BossAttackDamage(1.2f); BossAction.RemoveAllListeners(); });//����A�F�X
                        }
                        else if (a <= 60)
                        {
                            Debug.Log("����B�w��");
                            Vector2[] temp = new Vector2[5];
                            temp = Random5Block();
                            for (int i = 0; i < 5; i++)
                            {
                                Controller.GenWarning(temp[i]);
                                Controller.BlockReadyToBreak.Add(temp[i]);
                                Controller.GameMaster.GameMap.FindBlock(temp[i]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                            }
                            //����B�w��
                            BossAction.AddListener(() =>
                            {
                                Controller.BossAnimation(0, 1);
                                Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
                                Controller.BossAttackDamage(0.7f);
                                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                                {
                                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 1));
                                }
                                
                                Controller.BlockReadyToBreak = new List<Vector2>();
                                BossAction.RemoveAllListeners();
                            });//����B�F�X
                        }
                        else
                        {
                            TargetOneBlockMove(1f,1);
                            //����C�F�X
                        }
                    }
                    else
                    {
                        int a = Random.Range(0, 10);
                        if (a > 3)
                        {
                            int b = Random.Range(0, 2);
                            if (b==0)
                            {
                                Debug.Log("����D�w��");
                                int Row = Random.Range(0, 5);
                                for (int i = 0; i < 5; i++)
                                {
                                    Controller.GenWarning(new Vector2(Row, i));
                                    Controller.BlockReadyToBreak.Add(new Vector2(Row, i));
                                    Controller.GameMaster.GameMap.FindBlock(new Vector2(Row, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                //�B�~
                                List<Vector2> temp = new List<Vector2>();
                                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                                {
                                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                                    {
                                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                                        {
                                            temp.Add(new Vector2(j, i));
                                        }
                                    }
                                }
                                if (temp.Count > 0)
                                {
                                    int spot = Random.Range(0, temp.Count);
                                    int max = 0;
                                    for (int i = 0; i < 5; i++)
                                    {
                                        while (temp[spot] == new Vector2(Row, i))
                                        {
                                            spot = Random.Range(0, temp.Count);
                                            max++;
                                            if (max == temp.Count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    Controller.GenWarning(temp[spot]);
                                    Controller.BlockReadyToBreak.Add(temp[spot]);
                                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                BossAction.AddListener(() =>
                                {
                                    Controller.BossAnimation(0, 1);
                                    Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
                                    Controller.BossAttackDamage(0.6f);
                                    Controller.BossAttackDamage(0.6f);
                                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                                    {
                                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 2));
                                    }
                                    
                                    Controller.BlockReadyToBreak = new List<Vector2>();
                                    BossAction.RemoveAllListeners();
                                });
                            }
                            else
                            {
                                Debug.Log("����E�w��");
                                int Column = Random.Range(0, 5);
                                for (int i = 0; i < 5; i++)
                                {
                                    Controller.GenWarning(new Vector2(i, Column));
                                    Controller.BlockReadyToBreak.Add(new Vector2(i, Column));
                                    Controller.GameMaster.GameMap.FindBlock(new Vector2(i, Column)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                //�B�~
                                List<Vector2> temp = new List<Vector2>();
                                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                                {
                                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                                    {
                                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                                        {
                                            temp.Add(new Vector2(j, i));
                                        }
                                    }
                                }
                                if (temp.Count > 0)
                                {
                                    int spot = Random.Range(0, temp.Count);
                                    int max = 0;
                                    for (int i = 0; i < 5; i++)
                                    {
                                        while (temp[spot] == new Vector2(i, Column))
                                        {
                                            spot = Random.Range(0, temp.Count);
                                            max++;
                                            if (max == temp.Count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    Controller.GenWarning(temp[spot]);
                                    Controller.BlockReadyToBreak.Add(temp[spot]);
                                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                BossAction.AddListener(() =>
                                {
                                    Controller.BossAnimation(0, 1);
                                    Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
                                    Controller.BossAttackDamage(1.5f);
                                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                                    {
                                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 2));
                                    }
                                    
                                    Controller.BlockReadyToBreak = new List<Vector2>();
                                    BossAction.RemoveAllListeners();
                                });
                            }
                        }
                        else
                        {
                            if (a == 0)
                            {
                                Debug.Log("����A_UP�w��");
                                TargetOneBlockMove(1.2f,0);//����A�F�X
                            }
                            else if (a == 1)
                            {
                                Debug.Log("����B_UP�w��");
                                Vector2[] temp1 = new Vector2[5];
                                temp1 = Random5Block();
                                for (int i = 0; i < 5; i++)
                                {
                                    Controller.GenWarning(temp1[i]);
                                    Controller.BlockReadyToBreak.Add(temp1[i]);
                                    Controller.GameMaster.GameMap.FindBlock(temp1[i]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                List<Vector2> temp = new List<Vector2>();
                                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                                {
                                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                                    {
                                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                                        {
                                            temp.Add(new Vector2(j, i));
                                        }
                                    }
                                }
                                if (temp.Count > 0)
                                {
                                    int spot = Random.Range(0, temp.Count);
                                    int max = 0;
                                    for (int i = 0; i < temp1.Length; i++)
                                    {
                                        while (temp[spot] == temp1[i])
                                        {
                                            spot = Random.Range(0, temp.Count);
                                            max++;
                                            if (max == temp.Count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    Controller.GenWarning(temp[spot]);
                                    Controller.BlockReadyToBreak.Add(temp[spot]);
                                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                //����B�w��
                                BossAction.AddListener(() =>
                                {
                                    Controller.BossAnimation(0,1);
                                    Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
                                    Controller.BossAttackDamage(0.7f);
                                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                                    {
                                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 1));
                                    }
                                   
                                    Controller.BlockReadyToBreak = new List<Vector2>();
                                    BossAction.RemoveAllListeners();
                                });//����B�F�X
                            }
                            else
                            {
                                Debug.Log("����C_UP�w��");
                                List<Vector2> temp = new List<Vector2>();
                                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                                {
                                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                                    {
                                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                                        {
                                            temp.Add(new Vector2(j, i));
                                        }
                                    }
                                }
                                if (temp.Count > 0)
                                {
                                    int spot = Random.Range(0, temp.Count);
                                    int spot1 = Random.Range(0, temp.Count);
                                    while(temp.Count!=1&&spot == spot1)
                                    {
                                        spot1 = Random.Range(0, temp.Count);
                                    }
                                    Controller.GenWarning(temp[spot]);
                                    Controller.GenWarning(temp[spot1]);
                                    Controller.BlockReadyToBreak.Add(temp[spot]);
                                    Controller.BlockReadyToBreak.Add(temp[spot1]);
                                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                    Controller.GameMaster.GameMap.FindBlock(temp[spot1]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                }
                                BossAction.AddListener(() =>
                                {
                                    Controller.BossAnimation(0, 1);
                                    Controller.AttackUsedTime += Controller.BossAnimationTime(0, 1);
                                    Controller.BossAttackDamage(1f);
                                    if (Controller.BlockReadyToBreak.Count > 0)
                                    {
                                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                                        {
                                            Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, 1));
                                        }
                                    }
                                    
                                    Controller.BlockReadyToBreak = new List<Vector2>();
                                    BossAction.RemoveAllListeners();
                                });
                            }
                        }
                    }
                }
            }
        }

       
       
        
    }
    public void BossNormalAttack()
    {
        Debug.Log("�Ǫ�����");
        //�O�_�g��
        if (!RageSmash())
        {
            BossAction.Invoke();
        }
        Controller.CD_To_Next_Attack = 1;
    }
    
    public void BossSpecialAttack()
    {
        if (SpecialMove3 == true&&SpecialMove3Unmove == false)
        {
            Debug.Log("���������Ѱ�");
            SpecialMove3Unmove = true;
        }
    }    
    public bool RageSmash()
    {
        Vector2[] Temp = new Vector2[5];
        if (Controller.NowHealth<=Controller.MaxHealth/2f&&InRage == false)
        {
            //�i�g��
            InRage = true;
            Controller.ATK = 2000f;
            Controller.DEF = 1200f;
            //����
            int x = 0; int y = 0;
            for (int i = 0; i < 5; i++)
            {
                x = Random.Range(0, 5);
                y = Random.Range(0, 5);
                Temp[i] = new Vector2(x, y);
                for (int j = 0; j < i; j++)
                {
                    while (Temp[i]==Temp[j])
                    {
                        x = Random.Range(0, 5);
                        y = Random.Range(0, 5);
                        Temp[i] = new Vector2(x, y);
                    }
                }
            }
            for (int i = 0; i < Temp.Length; i++)
            {
                Controller.BossBreakSingleBlock(Temp[i], Controller.BossAnimationTime(0, 1));
            }
            //���ˮ`
            for (int i = 0; i < 5; i++)
            {
                Controller.BossAttackDamage(0.2f);
            }
            Controller.BossAnimation(0, 2);
            Controller.AttackUsedTime += Controller.BossAnimationTime(0, 2);
            Controller.WaitMoveFunction(() => { BossAction.Invoke();}, Controller.BossAnimationTime(0, 2));
            return true;
        }
        return false;
    }
    private Vector2[] Random5Block()
    {
        Vector2[] Temp = new Vector2[5];
        int x = 0; int y = 0;
        for (int i = 0; i < 5; i++)
        {
            x = Random.Range(0, 5);
            y = Random.Range(0, 5);
            Temp[i] = new Vector2(x, y);
            for (int j = 0; j < i; j++)
            {
                while (Temp[i] == Temp[j])
                {
                    x = Random.Range(0, 5);
                    y = Random.Range(0, 5);
                    Temp[i] = new Vector2(x, y);
                }
            }
        }
        return Temp;
    }
    private void TargetOneBlockMove(float Damage,int AnimationType)
    {
        List<Vector2> temp = new List<Vector2>();
        for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    temp.Add(new Vector2(j, i));
                }
            }
        }
        if (temp.Count > 0)
        {
            int spot = Random.Range(0, temp.Count);
            Controller.GenWarning(temp[spot]);
            Controller.BlockReadyToBreak.Add(temp[spot]);
            Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
        }
        BossAction.AddListener(() =>
        {
            Controller.BossAnimation(0, AnimationType);
            Controller.AttackUsedTime += Controller.BossAnimationTime(0, AnimationType);
            Controller.AttackUsedTime = 3f;
            Controller.BossAttackDamage(Damage);
            if (Controller.BlockReadyToBreak.Count > 0)
            {
                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                {
                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i], Controller.BossAnimationTime(0, AnimationType));
                }
            }
            
            Controller.BlockReadyToBreak = new List<Vector2>();
            BossAction.RemoveAllListeners();
        });
    }
}
