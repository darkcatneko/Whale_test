using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossIstate 
{
    BossController Controller { get; set; }
    void OnRoundEnter(BossController controller);
    void ResetRound();

    void BossChooseAttack();
    void BossNormalAttack();
    void BossSpecialAttack();
}

[System.Serializable]
public enum Boss
{
    WormBo,
    MainBoss,
}
