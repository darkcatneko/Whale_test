using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameState : Istate
{
    public GameController Controller { get; set; }
    private int[] TowerCount = new int[5];   
    public float RegenFunc(float BuffAmount)
    {
        ///
        bool[] Checker = new bool[Controller.W_Data.WeaponDataList.Count];
        for (int i = 0; i < Controller.m_MainPlayer.BringingWeaponID.Length; i++)
        {
            if (Controller.m_MainPlayer.BringingWeaponID[i] != 999)
            {
                Checker[Controller.m_MainPlayer.BringingWeaponID[i]] = true;
            }
        }
        bool WeaponActivate = false;
        for (int i = 0; i < Controller.WeaponSkillActivation.Length; i++)
        {
            if (Controller.WeaponSkillActivation[i])
            {
                WeaponActivate = true;
            }
        }
        if (Checker[9] && (WeaponActivate || Controller.m_MainPlayer.SkillActivation > 0))
        {
           float _buff = 0;
            switch (Controller.W_Data.WeaponDataList[9].Weapon_BreakLevel)
            {
                case 0:
                    _buff = 0.12f;
                    break;
                case 1:
                    _buff = 0.15f;
                    break;
                case 2:
                    _buff = 0.18f;
                    break;
                case 3:
                    _buff = 0.21f;
                    break;
                case 4:
                    _buff = 0.24f;
                    break;

            }
            return BuffAmount + _buff;
        }
        ///  
        return BuffAmount;
    }
    public float AttackVFXDelayTime = 0;
        public void OnStateEnter(GameController controller)
    {
        Debug.Log("¶i§ðÀ»");
        Controller = controller;

        controller.CallStartCoroutine(AttackCall());
        
    }
    public IEnumerator AttackCall()
    {
        int BlockNeedToCheck = 25;
        for (int i = 0; i < Controller.GameMap.ThisMap.Length; i++)
        {
            for (int j = 0; j < Controller.GameMap.ThisMap[i].ThisRow.Length; j++)
            {
                BlockNeedToCheck--;
                yield return null;
                if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    yield return new WaitForSeconds(0.2f);
                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType == WeaponEnum.Armor)
                    {
                        if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count > 0)
                        {
                            float BuffAmount = 0;
                            for (int b = 0; b < Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count; b++)
                            {
                                if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound > 0)
                                {
                                    BuffAmount += Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffAmount;
                                }
                            }
                            Debug.LogWarning(BuffAmount);
                            Controller.m_MainPlayer.Regenerate((int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, RegenFunc(BuffAmount));
                            for (int b = 0; b < Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count; b++)
                            {
                                if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound > 0)
                                {
                                    Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound -= 1;
                                }
                            }
                            for (int b = Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count - 1; b >= 0; b--)
                            {
                                if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound == 0)
                                {
                                    Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.RemoveAt(b);
                                }
                            }
                        }
                        else
                        {
                            Controller.m_MainPlayer.Regenerate((int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, RegenFunc(0));
                        }

                        if (Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft > 0)
                        {
                            Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft--;
                        }
                        TowerCount[0]++;
                    }
                    else
                    {
                        if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count > 0)
                        {
                            bool KnightBow = false;
                            foreach (var item in Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff)
                            {
                                if (item.BuffSpecialName == "KnightBow")
                                {
                                    KnightBow = true;
                                }
                            }
                            if (KnightBow)
                            {
                                float BuffAmount = 0;
                                for (int b = 0; b < Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count; b++)
                                {
                                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound > 0)
                                    {
                                        BuffAmount += Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffAmount;
                                    }
                                }
                                Debug.LogWarning(BuffAmount);
                                Controller.M_BossController.Be_Attack(Controller.m_MainPlayer.Attack * 0.8f, (int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType, Controller.m_MainPlayer.Buff_Amount + BuffAmount);
                                Controller.M_BossController.Be_Attack(Controller.m_MainPlayer.Attack * 0.8f, (int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType, Controller.m_MainPlayer.Buff_Amount + BuffAmount);
                                for (int b = 0; b < Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count; b++)
                                {
                                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound > 0)
                                    {
                                        Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound -= 1;
                                    }
                                }
                                for (int b = Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count - 1; b >= 0; b--)
                                {
                                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound == 0)
                                    {
                                        Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.RemoveAt(b);
                                    }
                                }
                            }
                            else
                            {
                                float BuffAmount = 0;
                                for (int b = 0; b < Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count; b++)
                                {
                                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound > 0)
                                    {
                                        BuffAmount += Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffAmount;
                                    }
                                }
                                Debug.LogWarning(BuffAmount);
                                Controller.M_BossController.Be_Attack(Controller.m_MainPlayer.Attack, (int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType, Controller.m_MainPlayer.Buff_Amount + BuffAmount);
                                for (int b = 0; b < Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count; b++)
                                {
                                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound > 0)
                                    {
                                        Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound -= 1;
                                    }
                                }
                                for (int b = Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.Count - 1; b >= 0; b--)
                                {
                                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff[b].BuffRound == 0)
                                    {
                                        Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockBuff.RemoveAt(b);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Controller.M_BossController.Be_Attack(Controller.m_MainPlayer.Attack, (int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel, Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType, Controller.m_MainPlayer.Buff_Amount);
                        }
                        if (Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft > 0)
                        {
                            Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft--;
                            ///
                            bool[] Checker = new bool[Controller.W_Data.WeaponDataList.Count];
                            for (int k = 0; k < Controller.m_MainPlayer.BringingWeaponID.Length; k++)
                            {
                                if (Controller.m_MainPlayer.BringingWeaponID[k] != 999)
                                {
                                    Checker[Controller.m_MainPlayer.BringingWeaponID[k]] = true;
                                }
                            }
                            if (Checker[8] && Controller.CanActivateWeapon8Passive && Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType == WeaponEnum.Penetrate)
                            {
                                int chance = 0;
                                switch (Controller.W_Data.WeaponDataList[8].Weapon_BreakLevel)
                                {
                                    case 0:
                                        chance = 40;
                                        break;
                                    case 1:
                                        chance = 50;
                                        break;
                                    case 2:
                                        chance = 60;
                                        break;
                                    case 3:
                                        chance = 70;
                                        break;
                                    case 4:
                                        chance = 80;
                                        break;
                                }
                                int r = UnityEngine.Random.Range(0, 100);
                                if (r < chance)
                                {
                                    Controller.GameMap.ThisMap[i].ThisRow[j].AmmoLeft++;
                                    Controller.CanActivateWeapon8Passive = false;
                                }
                            }

                            ///
                        }
                        TowerCount[(int)Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockType]++;
                    }
                }
            }
        }
        BossSpecialCheck();
        TowerCount = new int[5];
        if (BlockNeedToCheck ==0)
        {
            Controller.ChangeState(StateEnum.Defence_State);
        }       
    }
    public void OnStateStay()
    {

    }
    public void OnStateExit()
    {

    }
    public void BossSpecialCheck()
    {
        if (BossSpecialMoveDestroy(Boss.WormBo) && Controller.M_BossController.GetBoss() == Boss.WormBo)
        {
            Controller.M_BossController.BossSpecialAttack();
        }
        if (Controller.M_BossController.GetBoss() == Boss.MainBoss && Controller.M_BossController.NowSkillName == "SpecialAttackB")
        {
            for (int i = 1; i < TowerCount.Length; i++)
            {
                if (TowerCount[i] > 0)
                {
                    Controller.M_BossController.BossSpecialAttack();
                }
            }
        }
        if (Controller.M_BossController.GetBoss() == Boss.MainBoss && Controller.M_BossController.NowSkillName == "SpecialAttackE")
        {
            for (int i = 1; i < TowerCount.Length; i++)
            {
                if (TowerCount[i] > 0)
                {
                    Controller.M_BossController.BossSpecialAttack();
                }
            }
        }
    }
    
    public bool BossSpecialMoveDestroy(Boss boss)
    {
        bool complete = false;
        switch (boss)
        {
            case Boss.WormBo:                
                int max = 0;
                for (int i = 0; i < TowerCount.Length; i++)
                {
                    if (TowerCount[i]>0)
                    {
                        max++;
                    }
                }
                if (max>=3)
                {
                    complete = true;                    
                }
                return complete;          
        }
        Debug.Log("bruh");
        return complete;
    }
    
}
