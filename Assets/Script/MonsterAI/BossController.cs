using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameController GameMaster;
    private Dictionary<Boss, BossIstate> allBossDict;
    private Boss ThisRoundBoss;
    public List<Vector2> BlockReadyToBreak = new List<Vector2>();
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
            {Boss.WormBo,new WormBoAI()}
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
        Damage = Mathf.Clamp(Damage - DEF, 1, Damage);
        Damage = Mathf.RoundToInt(Damage);
        NowHealth -= Damage;
        Debug.Log(Damage);
    }
    public void BossAttackDamage(float Percentage)
    {
        GameMaster.m_MainPlayer.NowArmor -= Mathf.FloorToInt(Percentage * ATK);
        Debug.Log("Boss打出了" + Mathf.FloorToInt(Percentage * ATK) + "點傷害");
    }
}
