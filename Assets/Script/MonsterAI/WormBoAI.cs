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
    public void BossChooseAttack()
    {
        if (InRage == false)
        {
            int a = Random.Range(0, 100);
            if (a <= 30)
            {
                Debug.Log("����A�w��");
                BossAction.AddListener(() => { Controller.BossAttackDamage(1.2f); BossAction.RemoveAllListeners(); });//����A�F�X
            }
            else if (a <= 60)
            {
                Debug.Log("����B�w��");
                Vector2[] temp = new Vector2[5];
                temp = Random5Block();
                for (int i = 0; i < 5; i++)
                {
                    Controller.BlockReadyToBreak.Add(temp[i]);
                    Controller.GameMaster.GameMap.FindBlock(temp[i]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }
                //����B�w��
                BossAction.AddListener(() => 
                { 
                    Controller.BossAttackDamage(0.7f);
                    for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                    {
                        Controller.GameMaster.GameMap.DestroyAndRefreshSingleBlock(Controller.BlockReadyToBreak[i]);
                    }
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//����B�F�X
            }
            else
            {
                Debug.Log("����C�w��");
                List<Vector2> temp = new List<Vector2>();
                for (int i = 0; i < Controller.GameMaster.GameMap.ThisMap.Length; i++)
                {
                    for (int j = 0; j < Controller.GameMaster.GameMap.ThisMap[i].ThisRow.Length; j++)
                    {
                        if (Controller.GameMaster.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel>0)
                        {
                            temp.Add(new Vector2(j, i));
                        }
                    }
                }
                if (temp.Count>0)
                {
                    int spot = Random.Range(0, temp.Count);
                    Controller.BlockReadyToBreak.Add(temp[spot]);
                    Controller.GameMaster.GameMap.FindBlock(temp[spot]).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                }               
                //����C�w��
                BossAction.AddListener(() =>
                {
                    Controller.BossAttackDamage(1f);
                    if (Controller.BlockReadyToBreak.Count>0)
                    {
                        for (int i = 0; i < Controller.BlockReadyToBreak.Count; i++)
                        {
                            Controller.GameMaster.GameMap.DestroyAndRefreshSingleBlock(Controller.BlockReadyToBreak[i]);
                        }
                    }                    
                    Controller.BlockReadyToBreak = new List<Vector2>();
                    BossAction.RemoveAllListeners();
                });//����C�F�X
            }
        }
    }
    public void BossNormalAttack()
    {
        Debug.Log("�Ǫ�����");
        RageSmash();//�O�_�g��
        //�e�@�^�X�O�_���S��
        BossAction.Invoke();//�p�G�S�� ����
        //�w��


        Controller.CD_To_Next_Attack = 1;
    }
    public void BossSpecialAttack()
    {

    }
    public void RageSmash()
    {
        Vector2[] Temp = new Vector2[5];
        if (Controller.NowHealth<=Controller.MaxHealth/2f&&InRage == false)
        {
            //�i�g��
            InRage = true;
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
                Controller.GameMaster.GameMap.DestroyAndRefreshSingleBlock(Temp[i]);
            }
            //���ˮ`
            for (int i = 0; i < 5; i++)
            {
                Controller.BossAttackDamage(0.2f);
            }
        }
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
}
