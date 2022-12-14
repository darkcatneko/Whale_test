using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
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
    public MonsterData M_Data;
    public PlayableDirector TimeLine;
    #region Canvas
    public Button[] WeaponButton = new Button[5];
    public Button MainCharacterSkillButton;
    public Image BossHealth;
    public Image MyHealth;
    public Image EnergyBar;
    //public TextMeshProUGUI BossHealth;
    //public TextMeshProUGUI PlayerHealth;
    //public TextMeshProUGUI TPLog;
    //public TextMeshProUGUI StateLog;
    #endregion
    #region PCTEST
    public WeaponSO Test_1;

    #endregion
    #region GameUseMath
    public int HitCount = 0;
    public int TurnPoint = 0;
    public int MaxMP = 0;
    public int NowMP = 0;
    #endregion
    #region �@�^�X�@��
    public bool CanActivateWeapon8Passive = true;
    #endregion
    [SerializeField] public bool[] WeaponSkillActivation = new bool[15];



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
            {StateEnum.Ready_State, new ReadyTurnState()},
            {StateEnum.Skill_State, new SkillGameState()},
            {StateEnum.Animation_State, new AnimationState()}
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
        BossHealth.fillAmount = (float)M_BossController.NowHealth / M_BossController.MaxHealth;
        MyHealth.fillAmount = (float)m_MainPlayer.NowArmor  /  m_MainPlayer.MaxArmor;
        EnergyBar.fillAmount = (float)NowMP / (float)MaxMP;
        //BossHealth.text = M_BossController.NowHealth + " / " + M_BossController.MaxHealth;
        //PlayerHealth.text = m_MainPlayer.NowArmor + " / " + m_MainPlayer.MaxArmor;
        //StateLog.text = GetState().ToString();
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
        Debug.Log("����"+M_BossController.AttackUsedTime);
        yield return new WaitForSeconds(M_BossController.AttackUsedTime);
        M_BossController.BossIdleAnimation((int)ThisStage.ThisRoundBoss);
        yield return new WaitForSeconds(1.2f);
        M_BossController.AttackUsedTime = 0;
        if (M_BossController.GetBoss() == Boss.MainBoss&&M_BossController.NowSkillName == "SpecialAttackE")
        {
            bool CanHeal = false;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (GameMap.FindBlock(new Vector2(j, i)).ThisBlockLevel>0)
                    {
                        CanHeal = true;
                    }
                    
                }
            }
            if (CanHeal)
            {
                m_MainPlayer.NowArmor = Mathf.Clamp(m_MainPlayer.NowArmor + Mathf.FloorToInt(m_MainPlayer.MaxArmor * 0.5f),0, m_MainPlayer.MaxArmor);
            }
            M_BossController.NowSkillName = "";
        }
        Debug.LogWarning("�ǳƸ���");
        ChangeState(StateEnum.Ready_State);
    }
    public void ReadyTurnFunc()
    {
        StartCoroutine("ReadyTurn");
    }
    private IEnumerator ReadyTurn()
    {
        ///�@�^�X�@��
        M_BossController.Weapon5CanActivate = 2;
        M_BossController.Weapon6Passive = 3;
        GameMap.CanActivateWeapon14 = true;
        GameMap.CanActivateWeapon12 = true;
        ///
        int delay = 0;
        for (int i = 0; i < WeaponButton.Length; i++)
        {
            if (WeaponButton[i].GetComponent<WeaponButtonAction>().NowCoolDown< WeaponButton[i].GetComponent<WeaponButtonAction>().ButtonWeapon.m_WeaponCD)
            {
                WeaponButton[i].GetComponent<WeaponButtonAction>().NowCoolDown = Mathf.Clamp(WeaponButton[i].GetComponent<WeaponButtonAction>().NowCoolDown+1,1, WeaponButton[i].GetComponent<WeaponButtonAction>().ButtonWeapon.m_WeaponCD);
            }            
        }
        if (WeaponSkillActivation[14])
        {
            ///
            WeaponSkillActivation[14] = false;
            float buffamount = 0;
            switch (W_Data.WeaponDataList[14].Weapon_BreakLevel + 1)
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
            m_MainPlayer.Regen_Buff_Amount -= buffamount;
            Debug.LogWarning("�_�îĪG����"+ m_MainPlayer.Regen_Buff_Amount.ToString());
            ///
        }
        for (int i = 0; i < WeaponSkillActivation.Length; i++)//�Z���o�ʭ���
        {
            WeaponSkillActivation[i] = false;
        }
        //M_BossController.DestroyWarning();
        for (int i = 0; i < GameMap.ThisMap.Length; i++)
        {
            
            for (int j = 0; j < GameMap.ThisMap[i].ThisRow.Length; j++)
            {               
                if (CharacterReadyPassive(new Vector2(j, i)))
                {
                        delay++;
                }                
                if (GameMap.ThisMap[i].ThisRow[j].AmmoLeft <= 0 && GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel > 0)
                {
                    delay++;
                    GameMap.DestroyAndRefreshSingleBlock(new Vector2(j, i),0f);
                }
            }
        }
        if (delay>0)
        {
            yield return new WaitForSeconds(3);
            
            TurnPoint = GameMap.TurnPointGain(3,this);//����¦3
            M_BossController.BossChooseAttack();//���~����
            if (m_MainPlayer.SkillActivation>0)
            {
                m_MainPlayer.SkillActivation--;
            }
            ChangeState(StateEnum.Free_State);
        }
        else
        {
            
            TurnPoint = GameMap.TurnPointGain(3, this);//����¦3
            M_BossController.BossChooseAttack();//���~����
            if (m_MainPlayer.SkillActivation > 0)
            {
                m_MainPlayer.SkillActivation--;
            }
            ChangeState(StateEnum.Free_State);
        }
    }
    public bool CharacterReadyPassive(Vector2 Pos)
    {
        
        switch(m_MainPlayer.ThisRound_MainCharacter_ID)
        {
            case 0:                
                if (GameMap.FindBlock(Pos).ThisBlockType == WeaponEnum.Slash&& GameMap.FindBlock(Pos).ThisBlockLevel > 0&&m_MainPlayer.SkillActivation == 0)
                {                    
                    Destroy(GameMap.FindBlock(Pos).m_ThisBlockObject);
                    GameMap.SpawnSingleMapObject(WeaponEnum.Slash, (int)GameMap.FindBlock(Pos).ThisBlockLevel-1, (int)Pos.y, (int)Pos.x, GameMap.StartAmmo,0,0,0);
                }
                return true;
        }
        return false;
    }
    public void CallBossAttack()
    {
        StartCoroutine("BossAttackFullFunction");
    }
    private void OnApplicationQuit()
    {
        m_MainPlayer.Reset();
    }
    public StateEnum GetState()
    {
        return currentState;
    }
    public void CallStartCoroutine(IEnumerator Func)
    {
        StartCoroutine(Func);
    }    
}
