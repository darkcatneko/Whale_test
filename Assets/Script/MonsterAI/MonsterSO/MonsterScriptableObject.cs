using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
[System.Serializable]
[CreateAssetMenu(fileName = "NewMonsterSo", menuName = "MonsterSo")]
public class MonsterScriptableObject : ScriptableObject
{
    public string BossName;
    public List<PlayableAsset> BossAttacks = new List<PlayableAsset>();
}
