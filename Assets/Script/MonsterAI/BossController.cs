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
    #region 特效
    public List<GameObject> WarningPrefabs = new List<GameObject>();
    [SerializeField] GameObject WarningPrefab;
    #endregion
    #region 武器一回合一次
    public int Weapon5CanActivate = 2;
    public int Weapon6Passive = 3;
    #endregion
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
    public void CharacterPassiveEnergyCharge()
    {
        switch (GameMaster.m_MainPlayer.ThisRound_MainCharacter_ID)
        {
            case 3:
                GameMaster.NowMP = Mathf.Clamp(GameMaster.NowMP + 1, 0, GameMaster.MaxMP);
                return;
        }
    }
    public void Be_Attack(float Atk, int level, WeaponEnum type, float Buff)
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
        ///
        bool[] Checker = new bool[GameMaster.W_Data.WeaponDataList.Count];
        for (int i = 0; i < GameMaster.m_MainPlayer.BringingWeaponID.Length; i++)
        {
            if (GameMaster.m_MainPlayer.BringingWeaponID[i] != 999)
            {
                Checker[GameMaster.m_MainPlayer.BringingWeaponID[i]] = true;
            }
        }
        bool WeaponActivate = false;
        for (int i = 0; i < GameMaster.WeaponSkillActivation.Length; i++)
        {
            if (GameMaster.WeaponSkillActivation[i])
            {
                WeaponActivate = true;
            }
        }
        if (Checker[0] && type == WeaponEnum.Slash)
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[0].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.02f;
                    break;
                case 2:
                    buffamount = 0.025f;
                    break;
                case 3:
                    buffamount = 0.03f;
                    break;
                case 4:
                    buffamount = 0.035f;
                    break;
                case 5:
                    buffamount = 0.04f;
                    break;
            }
            Buff += buffamount;
        }
        if (Checker[1] && type == WeaponEnum.Lunge)
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[1].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.02f;
                    break;
                case 2:
                    buffamount = 0.025f;
                    break;
                case 3:
                    buffamount = 0.03f;
                    break;
                case 4:
                    buffamount = 0.035f;
                    break;
                case 5:
                    buffamount = 0.04f;
                    break;
            }
            Buff += buffamount;
        }
        if (Checker[2] && type == WeaponEnum.Hit)
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[2].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.02f;
                    break;
                case 2:
                    buffamount = 0.025f;
                    break;
                case 3:
                    buffamount = 0.03f;
                    break;
                case 4:
                    buffamount = 0.035f;
                    break;
                case 5:
                    buffamount = 0.04f;
                    break;
            }
            Buff += buffamount;
        }
        if (Checker[3] && type == WeaponEnum.Penetrate)
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[3].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.02f;
                    break;
                case 2:
                    buffamount = 0.025f;
                    break;
                case 3:
                    buffamount = 0.03f;
                    break;
                case 4:
                    buffamount = 0.035f;
                    break;
                case 5:
                    buffamount = 0.04f;
                    break;
            }
            Buff += buffamount;
        }
        if (Checker[12] && type == WeaponEnum.Hit)
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[12].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.06f;
                    break;
                case 2:
                    buffamount = 0.075f;
                    break;
                case 3:
                    buffamount = 0.09f;
                    break;
                case 4:
                    buffamount = 0.105f;
                    break;
                case 5:
                    buffamount = 0.12f;
                    break;
            }
            Buff += buffamount;
        }
        if (Checker[13])
        {
            int HitNeed = 0;
            switch (GameMaster.W_Data.WeaponDataList[13].Weapon_BreakLevel + 1)
            {
                case 1:
                    HitNeed = 9;
                    break;
                case 2:
                    HitNeed = 8;
                    break;
                case 3:
                    HitNeed = 7;
                    break;
                case 4:
                    HitNeed = 6;
                    break;
                case 5:
                    HitNeed = 5;
                    break;
            }
            GameMaster.HitCount++;
            if (GameMaster.HitCount == HitNeed)
            {
                GameMaster.HitCount = 0;
                GameMaster.NowMP = Mathf.Clamp((int)GameMaster.NowMP + 1, 0, (int)GameMaster.MaxMP);
            }
        }
        if (Checker[7]&& (WeaponActivate || GameMaster.m_MainPlayer.SkillActivation > 0))
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[7].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.12f;
                    break;
                case 2:
                    buffamount = 0.15f;
                    break;
                case 3:
                    buffamount = 0.18f;
                    break;
                case 4:
                    buffamount = 0.21f;
                    break;
                case 5:
                    buffamount = 0.24f;
                    break;
            }
            Buff += buffamount;
        }
        ///
        Damage = Atk * LevelAtk * resistance[(int)type - 1] * Buff;
        //Debug.LogError(Damage);
        Damage = CharacterPassiveCheck(type, Damage, level);
        //Debug.LogWarning(Damage);
        Damage = Mathf.Clamp(Damage - DEF, 1, Damage);
        Damage = Mathf.RoundToInt(Damage);
        CharacterPassiveExtraAttackCheck(Damage);
        CharacterPassiveEnergyCharge();
        NowHealth -= Damage;
        ///攻擊後
        if (Checker[11])
        {
            float buffamount = 0;
            switch (GameMaster.W_Data.WeaponDataList[3].Weapon_BreakLevel + 1)
            {
                case 1:
                    buffamount = 0.12f;
                    break;
                case 2:
                    buffamount = 0.15f;
                    break;
                case 3:
                    buffamount = 0.18f;
                    break;
                case 4:
                    buffamount = 0.21f;
                    break;
                case 5:
                    buffamount = 0.24f;
                    break;
            }
            int r = UnityEngine.Random.Range(1, 11);
            if (r < 4)
            {
                Debug.Log("回血成功");
                GameMaster.m_MainPlayer.NowArmor = Mathf.FloorToInt(Mathf.Clamp(GameMaster.m_MainPlayer.NowArmor + Damage * buffamount, 0, GameMaster.m_MainPlayer.MaxArmor));
            }


        }
        
        if ((Checker[5] && Weapon5CanActivate > 0) && (WeaponActivate||GameMaster.m_MainPlayer.SkillActivation>0))
        {
            int ManaGain = 0;
            switch (GameMaster.W_Data.WeaponDataList[5].Weapon_BreakLevel)
            {
                case 0:
                    ManaGain = 1;
                    break;
                case 1:
                    ManaGain = 1;
                    break;
                case 2:
                    ManaGain = 2;
                    break;
                case 3:
                    ManaGain = 2;
                    break;
                case 4:
                    ManaGain = 3;
                    break;
            }


            GameMaster.NowMP = Mathf.Clamp((int)GameMaster.NowMP + ManaGain, 0, (int)GameMaster.MaxMP);
            Weapon5CanActivate -= 1;
        }
        if (Checker[6]&&type == WeaponEnum.Lunge&& Weapon6Passive > 0)
        {
            int Chance = 0;
            switch (GameMaster.W_Data.WeaponDataList[6].Weapon_BreakLevel)
            {
                case 0:
                    Chance = 40;
                    break;
                case 1:
                    Chance = 50;
                    break;
                case 2:
                    Chance = 60;
                    break;
                case 3:
                    Chance = 70;
                    break;
                case 4:
                    Chance = 80;
                    break;
            }
            int r = UnityEngine.Random.Range(0, 100);
            if (r<Chance)
            {
                GameMaster.NowMP = Mathf.Clamp((int)GameMaster.NowMP + 1, 0, (int)GameMaster.MaxMP);
                Weapon6Passive -= 1;
            }
           
        }
        ///
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
    public void BossBreakSingleBlock(Vector2 Vc,float Time)
    {
        if (GameMaster.GameMap.FindBlock(Vc).ShieldLeft>0)
        {
            GameMaster.GameMap.FindBlock(Vc).ShieldLeft--;
            GameMaster.M_BossController.DestroyWarning();
        }
        else
        {
            GameMaster.GameMap.DestroyAndRefreshSingleBlock(Vc,Time);
        }       
    }
    public void BossBreakSingleBlock(Vector2 Vc)
    {
        if (GameMaster.GameMap.FindBlock(Vc).ShieldLeft > 0)
        {
            GameMaster.GameMap.FindBlock(Vc).ShieldLeft--;
            GameMaster.M_BossController.DestroyWarning();
        }
        else
        {
            GameMaster.GameMap.DestroyAndRefreshSingleBlock(Vc, 0);
        }
    }
    public void BossBreakSingleBlock_MutiHit(Vector2 Vc,int hit,float Time)
    {
        if(GameMaster.GameMap.FindBlock(Vc).ShieldLeft- hit > 0)
        {
            GameMaster.GameMap.FindBlock(Vc).ShieldLeft-= hit;
        }
        else
        {
            GameMaster.GameMap.DestroyAndRefreshSingleBlock(Vc, Time);
        }
    }
    public void BossBreakSingleBlock_MutiHit(Vector2 Vc, int hit)
    {
        if (GameMaster.GameMap.FindBlock(Vc).ShieldLeft - hit > 0)
        {
            GameMaster.GameMap.FindBlock(Vc).ShieldLeft -= hit;
        }
        else
        {
            GameMaster.GameMap.DestroyAndRefreshSingleBlock(Vc, 0);
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
    public void GenWarning(Vector2 Pos)
    {
       GameObject B = Instantiate(WarningPrefab, new Vector3(GameMaster.GameMap.MapStartPoint.transform.position.x + 1.2f * Pos.x, 0.53f, GameMaster.GameMap.MapStartPoint.transform.position.z + 1.2f * Pos.y), Quaternion.identity);
       WarningPrefabs.Add(B);
    }
    public void DestroyWarning()
    {
        if (WarningPrefabs.Count>0)
        {
            for (int i = 0; i < WarningPrefabs.Count; i++)
            {
                if (WarningPrefabs[i] != null)
                {
                    Destroy(WarningPrefabs[i]);
                }
            }
        }        
        WarningPrefabs = new List<GameObject>();
    }
    public void BossAnimation(int BossID, int MoveId)
    {
        GameMaster.TimeLine.extrapolationMode = UnityEngine.Playables.DirectorWrapMode.Hold;
        GameMaster.TimeLine.playableAsset = GameMaster.M_Data.MonsterList[BossID].BossAttacks[MoveId];
        GameMaster.TimeLine.Play();
    }
    public GameObject MonsterVFX(int BossID, int VFXID)
    {
        return GameMaster.M_Data.MonsterList[BossID].MonsterVFXPrefab[VFXID];
    }
    public void BossIdleAnimation(int BossID)
    {
        GameMaster.TimeLine.playableAsset = GameMaster.M_Data.MonsterList[BossID].BossIdle;
        GameMaster.TimeLine.Play();
        GameMaster.TimeLine.extrapolationMode = UnityEngine.Playables.DirectorWrapMode.Loop;
    }
    public float BossAnimationTime(int BossID, int MoveId)
    {
        return (float)GameMaster.M_Data.MonsterList[BossID].BossAttacks[MoveId].duration;
    }
}
