using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewMonsterData", menuName = "MonsterData")]
public class MonsterData : ScriptableObject
{
    public List<MonsterScriptableObject> MonsterList = new List<MonsterScriptableObject>();
}
