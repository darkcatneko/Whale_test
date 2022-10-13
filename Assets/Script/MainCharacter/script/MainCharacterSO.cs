using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MainCharacterSO", menuName = "NewCharacter")]
public class MainCharacterSO : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite CharacterImage;
    public int SkillUseMP;
    public UnityEvent BasicSkill;
    public UnityEvent PassiveSkill;
}
