using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
public class GameController : MonoBehaviour
{
    [SerializeField] private StateEnum currentState;
    private Dictionary<StateEnum, Istate> allStateDict;

    public BattleMap GameMap;
    public MainPlayer m_MainPlayer;
    public WeaponData W_Data;
    #region Canvas
    public Button[] WeaponButton = new Button[5];
    #endregion

    #region PCTEST
    public WeaponSO Test_1;

    #endregion   

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        allStateDict = new Dictionary<StateEnum, Istate>
        {
            {StateEnum.NOTHING, null},
            {StateEnum.Start_State, new StartGameState()},
            {StateEnum.Free_State, new FreeGameState()},
            {StateEnum.Hold_State, new HoldGameState()},
            {StateEnum.Attack_State, new AttackGameState()},
            {StateEnum.Defence_State, new DefenceGameState()},
            {StateEnum.Setting_State, new SettingGameState()}
        };

        ChangeState(StateEnum.Start_State);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        allStateDict[currentState]?.OnStateStay();
    }
    public void ChangeState(StateEnum newState)
    {
        allStateDict[currentState]?.OnStateExit();

        if (allStateDict[newState] == null) return;

        currentState = newState;
        allStateDict[currentState].OnStateEnter(this);
    }
    
}
