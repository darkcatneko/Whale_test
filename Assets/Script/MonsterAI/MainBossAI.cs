using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class MainBossAI : BossIstate
{
    private int SpecialMove_D_Counter = 0;
    public bool SpecialMoveA = false; public bool SpecialMoveB = false; public bool SpecialMoveC = false; public bool SpecialMoveE = false; public bool SpecialMoveF = false;
    public int SpecialMoveBHitType = 0; public int SpecialMoveCHitType = 0;
    public BossController Controller { get; set; }
    public UnityEvent BossAction = new UnityEvent();
    public void OnRoundEnter(BossController controller)
    {
        Controller = controller;
        Controller.Set_Stat(1, 100000f, 2000f, 1200f, 1.05f, 1.2f, 1.15f, 1f, 0.8f);
    }
    public void ResetRound()
    {
        Controller.CD_To_Next_Attack = 1;
    }
    public void BossChooseAttack_Normal()
    {
        if (Controller.NowHealth >= Controller.MaxHealth * 0.75f)
        {
            int x = Random.Range(0, 2);
            switch (x)
            {
                case 0:
                    Debug.Log("普攻A預兆");
                    int Biggest = FindBiggest();
                    List<Vector2> temp = new List<Vector2>();
                    for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                    {
                        for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                        {
                            if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                            {
                                temp.Add(new Vector2(j, i));
                            }
                        }
                    }
                    int spot = Random.Range(0, temp.Count);
                    Controller.BlockReadyToBreak.Add(temp[spot]);
                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    BossAction.AddListener(() =>
                    {
                        Controller.AttackUsedTime += 3f;
                        Controller.BossAttackDamage(1.2f);
                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                        {
                            Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                        }
                        Controller.BlockReadyToBreak = new List<Vector2>();
                        BossAction.RemoveAllListeners();
                    });//普攻A轟出
                    return;
                case 1:
                    Debug.Log("普攻B預兆");
                    List<Vector2> temp1 = new List<Vector2>();
                    for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                    {
                        for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                        {
                            if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                            {
                                temp1.Add(new Vector2(j, i));
                            }
                        }
                    }
                    if (temp1.Count > 0)
                    {
                        int spot2 = Random.Range(0, temp1.Count);
                        int spot1 = Random.Range(0, temp1.Count);
                        while (temp1.Count != 1 && spot2 == spot1)
                        {
                            spot1 = Random.Range(0, temp1.Count);
                        }
                        Controller.BlockReadyToBreak.Add(temp1[spot2]);
                        Controller.BlockReadyToBreak.Add(temp1[spot1]);
                        Controller.GameMaster.GameMap.FindBlock(temp1[spot2]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                        Controller.GameMaster.GameMap.FindBlock(temp1[spot1]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    }
                    BossAction.AddListener(() =>
                    {
                        Controller.AttackUsedTime += 3f;
                        Controller.BossAttackDamage(0.8f);
                        if (Controller.BlockReadyToBreak.Count > 0)
                        {
                            for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                            {
                                Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                            }
                        }
                        Controller.BlockReadyToBreak = new List<Vector2>();
                        BossAction.RemoveAllListeners();
                    });
                    return;
            }
        }
        else if (Controller.NowHealth >= Controller.MaxHealth * 0.5f && Controller.NowHealth < Controller.MaxHealth * 0.75f)
        {
            int x = Random.Range(0, 10);
            if (x < 7)
            {
                Debug.Log("普攻C預兆");
                int Biggest = FindBiggest();
                List<Vector2> temp = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                        {
                            temp.Add(new Vector2(j, i));
                        }
                    }
                }
                if (temp.Count > 1)
                {
                    int spot = Random.Range(0, temp.Count);
                    int spot2 = Random.Range(0, temp.Count);
                    while (spot2 == spot)
                    {
                        spot2 = Random.Range(0, temp.Count);
                    }
                    Controller.BlockReadyToBreak.Add(temp[spot]);
                    Controller.BlockReadyToBreak.Add(temp[spot2]);
                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    Controller.GameMaster.GameMap.FindBlock(temp[spot2]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    BossAction.AddListener(() =>
                    {
                        Controller.AttackUsedTime += 3f;
                        Controller.BossAttackDamage(1.2f);
                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                        {
                            Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                        }
                        Controller.BlockReadyToBreak = new List<Vector2>();
                        BossAction.RemoveAllListeners();
                    });//普攻C轟出
                }
                else if (temp.Count == 1 && Biggest > 1)
                {
                    float Second = 0;
                    for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                    {
                        for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                        {
                            if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > Second && Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel != Biggest)
                            {
                                Second = Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel;
                            }
                        }
                    }
                    for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                    {
                        for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                        {
                            if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Second && temp.Count < 2)
                            {
                                temp.Add(new Vector2(j, i));
                            }
                        }
                    }
                    for (int i = 0; i < temp.Count; i++)
                    {
                        Controller.BlockReadyToBreak.Add(temp[i]);
                        Controller.GameMaster.GameMap.FindBlock(temp[i]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    }
                    BossAction.AddListener(() =>
                    {
                        Controller.AttackUsedTime += 3f;
                        Controller.BossAttackDamage(1.35f);
                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                        {
                            Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                        }
                        Controller.BlockReadyToBreak = new List<Vector2>();
                        BossAction.RemoveAllListeners();
                    });//普攻C轟出
                }
            }
            else
            {
                Debug.Log("普攻D預兆");
                int Biggest = FindBiggest();
                List<Vector2> temp = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                        {
                            temp.Add(new Vector2(j, i));
                        }
                    }
                }
                int spot = Random.Range(0, temp.Count);
                for (int i = 0; i < 5; i++)
                {
                    Controller.BlockReadyToBreak.Add(new Vector2(i, temp[spot].y));
                    Controller.GameMaster.GameMap.FindBlock(new Vector2(i, temp[spot].y)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
                BossAction.AddListener(() =>
                {
                    Controller.AttackUsedTime += 3f;
                    Controller.BossAttackDamage(0.5f);
                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                    {
                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                    }
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//普攻D轟出
            }
        }
        else if (Controller.NowHealth >= Controller.MaxHealth * 0.3f && Controller.NowHealth < Controller.MaxHealth * 0.5f)
        {
            if (SpecialMove_D_Counter >= 5)
            {
                Debug.Log("特動D觸發");
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Controller.BlockReadyToBreak.Add(new Vector2(j, i));
                        Controller.GameMaster.GameMap.FindBlock(new Vector2(j, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    }
                }
                BossAction.AddListener(() =>
                {
                    Controller.AttackUsedTime += 3f;
                    int TurrentCount = 0;
                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                    {
                        if (Controller.GameMaster.GameMap.FindBlock(Controller.BlockReadyToBreak[i]).ThisBlockLevel > 0)
                        {
                            TurrentCount++;
                        }
                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                    }

                    Controller.BossAttackPercentageDamage(0.1f * TurrentCount);
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//特動D轟出
            }
            else
            {
                Debug.Log("普攻E預兆");
                int Biggest = FindBiggest();
                List<Vector2> temp = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                        {
                            temp.Add(new Vector2(j, i));
                        }
                    }
                }
                int spot = Random.Range(0, temp.Count);               
                Vector2[] Area = new Vector2[5] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
                for (int i = 0; i < Area.Length; i++)
                {
                    if (temp[spot].x+Area[i].x<5&& temp[spot].x + Area[i].x > 0&& temp[spot].y + Area[i].y < 5 && temp[spot].y + Area[i].y > 0)
                    {
                        Controller.BlockReadyToBreak.Add((temp[spot]+Area[i]));
                        Controller.GameMaster.GameMap.FindBlock((temp[spot] + Area[i])).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    }
                }
                SpecialMove_D_Counter++;
                BossAction.AddListener(() =>
                {
                    Controller.AttackUsedTime += 3f;
                    Controller.BossAttackDamage(0.1f);
                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                    {
                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                    }
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//普攻E轟出
            }
        }
        else if (Controller.NowHealth >= Controller.MaxHealth * 0.1f && Controller.NowHealth < Controller.MaxHealth * 0.3f)
        {
            int x = Random.Range(0, 10);
            if (x < 7)
            {
                Debug.Log("普攻E預兆");
                List<Vector2> temp1 = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                        {
                            temp1.Add(new Vector2(j, i));
                        }
                    }
                }
                if (temp1.Count > 0)
                {
                    int spot2 = Random.Range(0, temp1.Count);
                    int spot1 = Random.Range(0, temp1.Count);
                    while (temp1.Count != 1 && spot2 == spot1)
                    {
                        spot1 = Random.Range(0, temp1.Count);
                    }
                    Controller.BlockReadyToBreak.Add(temp1[spot2]);
                    Controller.BlockReadyToBreak.Add(temp1[spot1]);
                    Controller.GameMaster.GameMap.FindBlock(temp1[spot2]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    Controller.GameMaster.GameMap.FindBlock(temp1[spot1]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
                BossAction.AddListener(() =>
                {
                    Controller.AttackUsedTime += 3f;
                    Controller.BossAttackDamage(0.55f);
                    Controller.BossAttackDamage(0.55f);
                    if (Controller.BlockReadyToBreak.Count > 0)
                    {
                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                        {
                            Controller.BossBreakSingleBlock_MutiHit(Controller.BlockReadyToBreak[i], 2);
                        }
                    }
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });
            }
            else
            {
                Debug.Log("普攻G預兆");
                int Biggest = FindBiggest();
                List<Vector2> temp = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                        {
                            temp.Add(new Vector2(j, i));
                        }
                    }
                }
                int spot = Random.Range(0, temp.Count);
                for (int i = 0; i < 5; i++)
                {
                    Controller.BlockReadyToBreak.Add(new Vector2(temp[spot].x, i));
                    Controller.GameMaster.GameMap.FindBlock(new Vector2(temp[spot].x, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
                BossAction.AddListener(() =>
                {
                    Controller.AttackUsedTime += 3f;
                    Controller.BossAttackDamage(0.5f);
                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                    {
                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                    }
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//普攻G轟出
            }
        }
        else if (Controller.NowHealth < Controller.MaxHealth * 0.1f)
        {
            int Spot1 = Random.Range(0, 5);
            int Spot2 = Random.Range(0, 5);
            while (Spot2 == Spot1)
            {
                Spot2 = Random.Range(0, 5);
            }
            for (int i = 0; i < 5; i++)
            {
                Controller.BlockReadyToBreak.Add(new Vector2(i, Spot1));
                Controller.BlockReadyToBreak.Add(new Vector2(i, Spot2));
                Controller.GameMaster.GameMap.FindBlock(new Vector2(i, Spot1)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                Controller.GameMaster.GameMap.FindBlock(new Vector2(i, Spot2)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
            }
            
            BossAction.AddListener(() =>
            {
                Controller.AttackUsedTime += 3f;
                int TurrentCount = 0;
                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                {
                    if (Controller.GameMaster.GameMap.FindBlock(Controller.BlockReadyToBreak[i]).ThisBlockLevel > 0)
                    {
                        TurrentCount++;
                    }
                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                }

                Controller.BossAttackPercentageDamage(0.1f * TurrentCount);
                Controller.BlockReadyToBreak = new List<Vector2>();
                BossAction.RemoveAllListeners();
            });//普攻H轟出
        }
    }
    public void BossChooseAttack()
    {
        if (Controller.NowHealth > Controller.MaxHealth * 0.5f && Controller.NowHealth <= Controller.MaxHealth * 0.75f&&SpecialMoveA == false)
        {
            Debug.Log("特動A預兆");
            SpecialMoveA = true;
            int Biggest = FindBiggest();
            List<Vector2> temp = new List<Vector2>();
            for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
            {
                for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                {
                    if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                    {
                        temp.Add(new Vector2(j, i));
                    }
                }
            }
            int spot = Random.Range(0, temp.Count);
            Vector2[] Area = new Vector2[5] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
            for (int i = 0; i < Area.Length; i++)
            {
                if (temp[spot].x + Area[i].x < 5 && temp[spot].x + Area[i].x >= 0 && temp[spot].y + Area[i].y < 5 && temp[spot].y + Area[i].y >= 0)
                {
                    Controller.BlockReadyToBreak.Add((temp[spot] + Area[i]));
                    Controller.GameMaster.GameMap.FindBlock((temp[spot] + Area[i])).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
            }
           
            BossAction.AddListener(() =>
            {
                Controller.AttackUsedTime += 3f;
                int TurrentCount = 0;
                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                {
                    if (Controller.GameMaster.GameMap.FindBlock(Controller.BlockReadyToBreak[i]).ThisBlockLevel > 0)
                    {
                        TurrentCount++;
                    }
                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                }

                Controller.BossAttackPercentageDamage(0.1f * TurrentCount);
                Controller.BlockReadyToBreak = new List<Vector2>();
                BossAction.RemoveAllListeners();
            });//特動A轟出
        }
        else
        {
            if (Controller.NowHealth > Controller.MaxHealth * 0.4f && Controller.NowHealth <= Controller.MaxHealth * 0.5f && SpecialMoveB == false)
            {
                Debug.Log("特動B預兆");
                SpecialMoveB = true;
                Controller.NowSkillName = "SpecialAttackB";
                int Biggest = FindBiggest();
                List<Vector2> temp = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                        {
                            temp.Add(new Vector2(j, i));
                        }
                    }
                }
                int spot = Random.Range(0, temp.Count);
                for (int i = 0; i < 5; i++)
                {
                    Controller.BlockReadyToBreak.Add(new Vector2(temp[spot].x, i));
                    Controller.BlockReadyToBreak.Add(new Vector2(i, temp[spot].y));
                    Controller.GameMaster.GameMap.FindBlock(new Vector2(temp[spot].x, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    Controller.GameMaster.GameMap.FindBlock(new Vector2(i, temp[spot].y)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
                BossAction.AddListener(() =>
                {
                    Controller.AttackUsedTime += 3f;
                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                    {                       
                        Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                    }
                    Controller.BossAttackPercentageDamage(0.8f-Mathf.Clamp(SpecialMoveBHitType,0,4)*0.1f);
                    Controller.NowSkillName = "";
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//特動B轟出
            }
            else
            {
                if (Controller.NowHealth > Controller.MaxHealth * 0.3f && Controller.NowHealth <= Controller.MaxHealth * 0.4f && SpecialMoveC == false)
                {
                    Debug.Log("特動C預兆");
                    Controller.NowSkillName = "SpecialAttackC";
                    SpecialMoveC = true;
                    int Biggest = FindBiggest();
                    List<Vector2> temp = new List<Vector2>();
                    for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                    {
                        for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                        {
                            if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == Biggest)
                            {
                                temp.Add(new Vector2(j, i));
                            }
                        }
                    }
                    int spot = Random.Range(0, temp.Count);
                    for (int i = 0; i < 5; i++)
                    {
                        Controller.BlockReadyToBreak.Add(new Vector2(temp[spot].x, i));
                        Controller.BlockReadyToBreak.Add(new Vector2(i, temp[spot].y));
                        Controller.GameMaster.GameMap.FindBlock(new Vector2(temp[spot].x, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                        Controller.GameMaster.GameMap.FindBlock(new Vector2(i, temp[spot].y)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    }
                    BossAction.AddListener(() =>
                    {
                        Controller.AttackUsedTime += 3f;
                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                        {
                            Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                        }
                        Controller.BossAttackPercentageDamage(1f - Mathf.Clamp(SpecialMoveCHitType, 0, 4) * 0.1f);
                        Controller.NowSkillName = "";
                        Controller.BlockReadyToBreak = new List<Vector2>();
                        BossAction.RemoveAllListeners();
                    });//特動C轟出
                }
                else
                {
                    if (Controller.NowHealth > Controller.MaxHealth * 0.1f && Controller.NowHealth <= Controller.MaxHealth * 0.3f && SpecialMoveE == false)
                    {
                        Debug.Log("特動E預兆");
                        SpecialMoveE = true;
                        Controller.ATK = 2500;
                        Controller.DEF = 1400;
                        Controller.NowSkillName = "SpecialAttackE";
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                Controller.BlockReadyToBreak.Add(new Vector2(j, i));
                                Controller.GameMaster.GameMap.FindBlock(new Vector2(j, i)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                            }
                        }
                        BossAction.AddListener(() =>
                        {
                            Controller.AttackUsedTime += 3f;
                            int TurrentCount = 0;
                            for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                            {
                                if (Controller.GameMaster.GameMap.FindBlock(Controller.BlockReadyToBreak[i]).ThisBlockLevel > 0)
                                {
                                    TurrentCount++;
                                }
                                Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                            }
                            Controller.BossAttackDamage(0.1f);
                            Controller.BossAttackPercentageDamage(0.1f * TurrentCount);
                            Controller.BlockReadyToBreak = new List<Vector2>();
                            BossAction.RemoveAllListeners();
                        });//特動E轟出
                    }
                    else
                    {
                        if (Controller.NowHealth > 0 && Controller.NowHealth <= Controller.MaxHealth * 0.1f && SpecialMoveF == false)
                        {
                            Debug.Log("特動F預兆");
                            SpecialMoveF = true;
                            for (int i = 0; i < 5; i++)
                            {
                                Controller.BlockReadyToBreak.Add(new Vector2(i, 0));
                                Controller.BlockReadyToBreak.Add(new Vector2(i, 2));
                                Controller.BlockReadyToBreak.Add(new Vector2(i, 4));
                                Controller.GameMaster.GameMap.FindBlock(new Vector2(i, 0)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                Controller.GameMaster.GameMap.FindBlock(new Vector2(i, 2)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                                Controller.GameMaster.GameMap.FindBlock(new Vector2(i, 4)).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                            }
                            BossAction.AddListener(() =>
                            {
                                Controller.AttackUsedTime += 3f;                                
                                for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                                {
                                    Controller.BossBreakSingleBlock(Controller.BlockReadyToBreak[i]);
                                }

                                Controller.BossAttackPercentageDamage(0.5f);
                                Controller.BlockReadyToBreak = new List<Vector2>();
                                BossAction.RemoveAllListeners();
                            });//特動F轟出
                        }
                        else
                        {
                            BossChooseAttack_Normal();
                        }
                    }
                }
            }
        }
    }
    public void BossNormalAttack()
    {
        BossAction.Invoke();    
        Controller.CD_To_Next_Attack = 1;
    }
    public void BossSpecialAttack()
    {
        if (SpecialMoveB == true)
        {
            SpecialMoveBHitType++;
        }
        if (SpecialMoveC == true)
        {
            SpecialMoveCHitType++;
        }
    }
    public int FindBiggest()
    {
        float Biggest = 0;
        for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > Biggest)
                {
                    Biggest = Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel;
                }
            }
        }
        return (int)Biggest;
    }
}
