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
    public Camera MainCam;
    public BossController M_BossController;
    public StageInfoSO ThisStage;

    public BattleMap GameMap;
    public MainPlayer m_MainPlayer;
    public WeaponData W_Data;
    public MainCharacterData C_Data;    
    #region Canvas
    public Button[] WeaponButton = new Button[5];
    public TextMeshProUGUI BossHealth;
    public TextMeshProUGUI PlayerHealth;
    public TextMeshProUGUI DamageLog;
    #endregion
    #region PCTEST
    public WeaponSO Test_1;

    #endregion
    #region GameUseMath
    public int TurnPoint = 0;
    public int MaxMP = 0;
    public int NowMP = 0;   
    #endregion



    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        MainCam = Camera.main;
        allStateDict = new Dictionary<StateEnum, Istate>
        {
            {StateEnum.NOTHING, null},
            {StateEnum.Start_State, new StartGameState()},
            {StateEnum.Free_State, new FreeGameState()},
            {StateEnum.Hold_State, new HoldGameState()},
            {StateEnum.Attack_State, new AttackGameState()},
            {StateEnum.Defence_State, new DefenceGameState()},
            {StateEnum.Setting_State, new SettingGameState()},
            {StateEnum.Ready_State, new ReadyTurnState()}
        };       
    }

    void Start()
    {
        ChangeState(StateEnum.Start_State);
    }

    // Update is called once per frame
    void Update()
    {
        allStateDict[currentState]?.OnStateStay();
        BossHealth.text = M_BossController.NowHealth + " / " + M_BossController.MaxHealth;
        PlayerHealth.text = m_MainPlayer.NowArmor + " / " + m_MainPlayer.MaxArmor;
    }
    public void ChangeState(StateEnum newState)
    {
        allStateDict[currentState]?.OnStateExit();

        if (allStateDict[newState] == null) return;

        currentState = newState;
        allStateDict[currentState].OnStateEnter(this);
    }
    public IEnumerator BossAttackFullFunction()
    {
        M_BossController.BossNormalAttack();
        Debug.Log("©µ®É"+M_BossController.AttackUsedTime);
        yield return new WaitForSeconds(M_BossController.AttackUsedTime);
        M_BossController.AttackUsedTime = 0;
        ChangeState(StateEnum.Ready_State);
    }
    public void CallBossAttack()
    {
        StartCoroutine("BossAttackFullFunction");
    }
    private void OnApplicationQuit()
    {
        m_MainPlayer.Reset();
    }
}
