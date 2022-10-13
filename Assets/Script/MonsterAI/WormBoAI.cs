using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBoAI : BossIstate
{
    //²Ä¤@°¦Boss
    public BossController Controller { get; set; }
    public bool InRage = false;
    public void OnRoundEnter(BossController controller)
    {
        Controller = controller;
        Controller.Set_Stat(1, 50000f, 800f, 700f, 0.8f, 1f, 1.2f, 0.9f, 1);
    }
    public void ResetRound()
    {
        Controller.CD_To_Next_Attack = 1;
    }
    public void BossNormalAttack()
    {
        Debug.Log("©Çª«§ðÀ»");
        
        Controller.GameMaster.m_MainPlayer.NowArmor -= 50;
        Controller.CD_To_Next_Attack = 1;
    }
    public void BossSpecialAttack()
    {

    }
}
