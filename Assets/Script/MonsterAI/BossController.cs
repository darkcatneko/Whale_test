using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
public class BossController : MonoBehaviour
{
    public GameController GameMaster;
    private Dictionary<Boss, BossIstate> allBossDict;
    private Boss ThisRoundBoss;
    public List<Vector2> BlockReadyToBreak = new List<Vector2>();
    public float AttackUsedTime;
    public string NowSkillName = "";
    private UnityEvent DelayUse = new UnityEvent();
    #region BossStat
    public int CD_To_Next_Attack;
    public float MaxHealth; public float NowHealth;
    public float ATK; public float DEF;
    public float[] resistance = new float[5];
    #endregion

    private void Awake()
    {
        allBossDict = new Dictionary<Boss, BossIstate>
        {
            {Boss.WormBo,new WormBoAI()},
            {Boss.MainBoss,new MainBossAI()}
        };
    }
    public void SetBossData(Boss WhichBoss)
    {
        allBossDict[WhichBoss]?.OnRoundEnter(this);
        ThisRoundBoss = WhichBoss;
    }
    public void ResetCD()
    {
    }
    public void BossChooseAttack()
    {
        allBossDict[ThisRoundBoss]?.BossChooseAttack();
    }
    public void BossNormalAttack()
    {
        allBossDict[ThisRoundBoss]?.BossNormalAttack();
    }
    public void BossSpecialAttack()
    {
        allBossDict[ThisRoundBoss]?.BossSpecialAttack();
    }
    public void Set_Stat(int CD, float Health, float atk, float def, float res0, float res1, float res2, float res3, float res4)
    {
        CD_To_Next_Attack = CD;
        MaxHealth = Health;
        NowHealth = Health;
        ATK = atk;
        DEF = def;
        resistance[0] = res0;
        resistance[1] = res1;
        resistance[2] = res2;
        resistance[3] = res3;
        resistance[4] = res4;
    }
    public void Be_Attack(float Atk,int level,WeaponEnum type,float Buff )
    {
        float Damage = 0;
        float LevelAtk = 0;
        switch (level)
        {
            case 1:
                LevelAtk = 0.8f;
                break;
            case 2:
                LevelAtk = 2f;
                break;
            case 3:
                LevelAtk = 4f;
                break;
            case 4:
                LevelAtk = 7f;
                break;
            case 5:
                LevelAtk = 10.5f;
                break;
        }
        Damage = Atk * LevelAtk * resistance[(int)type-1] * Buff ;
        //Debug.LogError(Damage);
        Damage = CharacterPassiveCheck(type, Damage,level);
        //Debug.LogWarning(Damage);
        Damage = Mathf.Clamp(Damage - DEF, 1, Damage);
        Damage = Mathf.RoundToInt(Damage);
        NowHealth -= Damage;
        CharacterPassiveExtraAttackCheck(Damage);
        Debug.Log("我打出了" + Damage + "點傷害");
    }
    public float CharacterPassiveCheck(WeaponEnum DamageType,float Damage,float WeaponLevel)
    {
        switch(GameMaster.m_MainPlayer.ThisRound_MainCharacter_ID)
        {
            case 0:
                if (DamageType == WeaponEnum.Slash)
                {
                    return Damage * 1.5f;
                }
                return Damage;
            case 2:
                
                if (WeaponLevel<=3&&WeaponLevel>=1)
                {
                    return Damage * 0.65f;
                }
                return Damage;
        }
        return Damage;
    }
    public void CharacterPassiveExtraAttackCheck(float Damage)
    {
        switch (GameMaster.m_MainPlayer.ThisRound_MainCharacter_ID)
        {
            case 3:
                if (GameMaster.NowMP>=30||GameMaster.m_MainPlayer.SkillActivation>0)
                {
                    //Debug.LogError("追傷" + Mathf.FloorToInt(Damage * 0.3f));
                    NowHealth -= Mathf.FloorToInt(Damage * 0.3f);
                }
               
                return;
        }
    }
    public void BossAttackDamage(float Percentage)
    {
        GameMaster.m_MainPlayer.NowArmor -= Mathf.FloorToInt(Percentage * ATK);
        Debug.Log("Boss打出了" + Mathf.FloorToInt(Percentage * ATK) + "點傷害");
    }
    public void BossAttackPercentageDamage(float Percentage)
    {
        GameMaster.m_MainPlayer.NowArmor -= Mathf.FloorToInt(Percentage * GameMaster.m_MainPlayer.MaxArmor);
        Debug.Log("Boss打出了" + Mathf.FloorToInt(Percentage * GameMaster.m_MainPlayer.MaxArmor) + "點傷害");
    }
    public void BossBreakSingleBlock(Vector2 Vc)
    {
        if (GameMaster.GameMap.FindBlock(Vc).ShieldLeft>0)
        {
            GameMaster.GameMap.FindBlock(Vc).ShieldLeft--;
        }
        else
        {
            GameMaster.GameMap.DestroyAndRefreshSingleBlock(Vc);
        }       
    }
    public void BossBreakSingleBlock_MutiHit(Vector2 Vc,int hit)
    {
        if(GameMaster.GameMap.FindBlock(Vc).ShieldLeft- hit > 0)
        {
            GameMaster.GameMap.FindBlock(Vc).ShieldLeft-= hit;
        }
        else
        {
            GameMaster.GameMap.DestroyAndRefreshSingleBlock(Vc);
        }
    }
    public Boss GetBoss()
    {
        return ThisRoundBoss;
    }
    public void WaitMoveFunction(Action ac, float Time)
    {
        DelayUse.AddListener(()=> { ac();DelayUse.RemoveAllListeners(); });
        Invoke("WaitMove", Time);
    }
    public void WaitMove()
    {
        DelayUse.Invoke();
        Debug.Log("Bruh");
    }
}
