using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class MainSceneUiController : MonoBehaviour
{
    public NowScreen m_NowScreen = NowScreen.MainMenu;

    public PlayerData Data;
    public PlayerDataSystem DataSystem;
    public WeaponData W_Date;
    public MainCharacterData MC_Data;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI GemText;
    private Vector3 MainPanelStartPos;
    private Vector3 BuyStartPos;
    private bool Buying; private bool isFlying = false;
    #region 圖標
    //商店 = 0 倉庫 = 1 戰鬥 = 2
    public Sprite[] BarIcon = new Sprite[3];
    #endregion
    [SerializeField] GameObject MainPanel;[SerializeField] GameObject GachaPanel;[SerializeField] GameObject W_and_CPanel;[SerializeField] GameObject BattlePanel;[SerializeField] GameObject BuyPanel;

    [SerializeField] Image RightBarButtonImage;
    [SerializeField] Image LeftBarButtonImage;
    #region Index細項
    [SerializeField] IndexController indexController;
    [SerializeField] Image CharacterBar;
    public int Weapon_OR_CHA = 0;
    #endregion
    [SerializeField] GameObject[] ChaPrefabs;
    void Start()
    {
        MainPanelStartPos = MainPanel.transform.position;
        BuyStartPos = BuyPanel.transform.position;
        CharacterBar.sprite = MC_Data.MainCharacterDataList[Data.ThisAccount.NowMainCharactor].CharacterBar;
        indexController.CardInstall();
        WeaponCardInstall();
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = Data.ThisAccount.CoinCount.ToString();
        GemText.text = Data.ThisAccount.GemCount.ToString();

    }
    public void ShopButtonTest()
    {
        RightBarButtonImage.sprite = BarIcon[1];
        LeftBarButtonImage.sprite = BarIcon[2];
        GachaPanel.SetActive(true);
        BasicFadeOut(MainPanel, MainPanelStartPos, NowScreen.ShopMenu);
    }
    public void BattleButtonTest()
    {
        RightBarButtonImage.sprite = BarIcon[1];
        LeftBarButtonImage.sprite = BarIcon[0];
        BattlePanel.SetActive(true);
        BasicFadeOut(MainPanel, MainPanelStartPos, NowScreen.BattleMenu);
    }
    public void IndexButtonTest()
    {
        RightBarButtonImage.sprite = BarIcon[0];
        LeftBarButtonImage.sprite = BarIcon[2];
        IndexInstall();
        W_and_CPanel.SetActive(true);
        BasicFadeOut(MainPanel, MainPanelStartPos, NowScreen.Weapon_and_CharacterMenu);
    }
    public void IndexInstall()
    {
        if (Weapon_OR_CHA == 1)
        {
            Weapon_OR_CHA = 1;
            indexController.horizontal.spacing = -40;
            indexController.horizontal.padding.left = 0;
            indexController.IndexTopBar.sprite = indexController.w_Cha_top[1];
            for (int i = 0; i < indexController.IndexContent.transform.childCount; i++)
            {
                Destroy(indexController.IndexContent.transform.GetChild(i).gameObject);
            }
            for (int i = ChaPrefabs.Length - 1; i >= 0; i--)
            {
                GameObject OBJ = Instantiate(ChaPrefabs[i], indexController.IndexContent.transform);
                if (!Data.ThisAccount.HaveCharactorOrNot[i])
                {
                    OBJ.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                }
                OBJ.GetComponent<C_Button>().ThisButtonID = i;
                OBJ.GetComponent<Button>().onClick.AddListener(() => { int x = OBJ.GetComponent<C_Button>().ThisButtonID; ChangeCharacter(x); });
            }
            indexController.IndexContent.transform.Translate(new Vector3(5000, 0, 0));
        }
        else
        {
            Weapon_OR_CHA = 0;
            indexController.horizontal.spacing = 100;
            indexController.horizontal.padding.left = 128;
            indexController.IndexTopBar.sprite = indexController.w_Cha_top[0];
            for (int i = 0; i < indexController.IndexContent.transform.childCount; i++)
            {
                Destroy(indexController.IndexContent.transform.GetChild(i).gameObject);
            }
            for (int i = W_Date.WeaponDataList.Count - 1; i >= 0; i--)
            {
                //Debug.Log(123);
                GameObject OBJ = Instantiate(indexController.WeaponPrefab, indexController.IndexContent.transform);
                if (!Data.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot)
                {
                    OBJ.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                }
                OBJ.GetComponent<Image>().sprite = W_Date.WeaponDataList[i].WeaponBarImage;
                OBJ.GetComponent<WeaponIndex>().ThisWeaponID = i;
                OBJ.GetComponent<WeaponIndex>().Unlock = Data.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot;
                OBJ.GetComponent<Button>().onClick.AddListener(() => { int x = OBJ.GetComponent<WeaponIndex>().ThisWeaponID; WeaponCardPlugin(x); });
                OBJ.GetComponentsInChildren<Button>()[1].onClick.AddListener(() =>
                {
                    indexController.InfoPanel.SetActive(true);
                    indexController.InfoPanel.GetComponent<Image>().sprite = W_Date.WeaponDataList[OBJ.GetComponent<WeaponIndex>().ThisWeaponID].WeaponInfoImage;
                });
            }
            indexController.IndexContent.transform.Translate(new Vector3(5000, 0, 0));
        }
    }
    public void ChangeIndexFocus()
    {
        if (Weapon_OR_CHA == 0)
        {
            Weapon_OR_CHA = 1;
            indexController.horizontal.spacing = -40;
            indexController.horizontal.padding.left =0;
            indexController.IndexTopBar.sprite = indexController.w_Cha_top[1];
            for (int i = 0; i < indexController.IndexContent.transform.childCount; i++)
            {
                Destroy(indexController.IndexContent.transform.GetChild(i).gameObject);
            }
            for (int i = ChaPrefabs.Length - 1; i >= 0; i--)
            {
                GameObject OBJ = Instantiate(ChaPrefabs[i], indexController.IndexContent.transform);
                if (!Data.ThisAccount.HaveCharactorOrNot[i])
                {
                    OBJ.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                }
                OBJ.GetComponent<C_Button>().ThisButtonID = i;
                OBJ.GetComponent<Button>().onClick.AddListener(() => {int x = OBJ.GetComponent<C_Button>().ThisButtonID;  ChangeCharacter(x); });
            }
            indexController.IndexContent.transform.Translate(new Vector3(5000, 0, 0));
        }
        else
        {
            Weapon_OR_CHA = 0;
            indexController.horizontal.spacing = 100;
            indexController.horizontal.padding.left =128;
            indexController.IndexTopBar.sprite = indexController.w_Cha_top[0];
            for (int i = 0; i < indexController.IndexContent.transform.childCount; i++)
            {
                Destroy(indexController.IndexContent.transform.GetChild(i).gameObject);
            }
            for (int i = W_Date.WeaponDataList.Count-1; i >= 0; i--)
            {
                //Debug.Log(123);
                GameObject OBJ = Instantiate(indexController.WeaponPrefab, indexController.IndexContent.transform);              
                if (!Data.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot)
                {
                    OBJ.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                }
                OBJ.GetComponent<Image>().sprite = W_Date.WeaponDataList[i].WeaponBarImage;
                OBJ.GetComponent<WeaponIndex>().ThisWeaponID = i;
                OBJ.GetComponent<WeaponIndex>().Unlock = Data.ThisAccount.M_WeaponSaveFile[i].HaveWeaponOrNot;
                OBJ.GetComponent<Button>().onClick.AddListener(() => { int x = OBJ.GetComponent<WeaponIndex>().ThisWeaponID; WeaponCardPlugin(x);  });
                OBJ.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => 
                {
                    indexController.InfoPanel.SetActive(true);
                    indexController.InfoPanel.GetComponent<Image>().sprite = W_Date.WeaponDataList[OBJ.GetComponent<WeaponIndex>().ThisWeaponID].WeaponInfoImage;
                });
            }
            indexController.IndexContent.transform.Translate(new Vector3(5000, 0, 0));
        }
    }
    public void WeaponCardInstall()
    {
        for (int i = 0; i < Data.ThisAccount.WeaponBackpack.Length; i++)
        {
            if (Data.ThisAccount.WeaponBackpack[i]!=999)
            {
                indexController.cards[i].sprite = W_Date.WeaponDataList[Data.ThisAccount.WeaponBackpack[i]].WeaponCardImage;
            }
        }
    }
    public void WeaponCardPlugin(int weaponID)
    {
        if (indexController.NowFoucsWeapon<5&&Data.ThisAccount.M_WeaponSaveFile[weaponID].HaveWeaponOrNot)
        {
            for (int i = 0; i < indexController.NowFoucsWeapon; i++)
            {
                if (Data.ThisAccount.WeaponBackpack[i] == weaponID)
                {
                    return;
                }
            }
            Data.ThisAccount.WeaponBackpack[indexController.NowFoucsWeapon] = weaponID;
            indexController.NowFoucsWeapon = Mathf.Clamp(indexController.NowFoucsWeapon + 1, 0, 5);
            WeaponCardInstall();
        }       
    }
    public void WeaponCardRefresh()
    {
        for (int i = 0; i < Data.ThisAccount.WeaponBackpack.Length; i++)
        {
            Data.ThisAccount.WeaponBackpack[i] = 999;
            indexController.NowFoucsWeapon = 0;
            indexController.cards[i].sprite = indexController.cardORI[i];
        }
    }
    public void MainMenuButton()
    {
        if (m_NowScreen != NowScreen.MainMenu)
        {
            BasicFadeIn(MainPanel, MainPanelStartPos, NowScreen.MainMenu);
        }
    }
    public void BuyGemButton()
    {
        if (isFlying == false)
        {
            if (Buying)
            {
                isFlying = true;
                DOTween.To(() => BuyPanel.transform.position, x => BuyPanel.transform.position = x, BuyPanel.transform.position + new Vector3(0, -2000, 0), 1f).OnComplete(() => { BuyPanel.SetActive(false); BuyPanel.transform.position = BuyStartPos; Buying = false; isFlying = false; });
                foreach (var item in BuyPanel.GetComponentsInChildren<Image>())
                {
                    item.DOFade(0, 1f).OnComplete(() => { item.color = new Color(1, 1, 1, 1); });
                }
            }
            else
            {
                isFlying = true;
                BuyPanel.transform.position = BuyPanel.transform.position - new Vector3(0, -2000, 0);
                BuyPanel.SetActive(true);
                DOTween.To(() => BuyPanel.transform.position, x => BuyPanel.transform.position = x, BuyStartPos, 1f).OnComplete(() =>
                {
                    isFlying = false;
                    BuyPanel.transform.position = BuyStartPos;
                    Buying = true;
                });
                foreach (var item in BuyPanel.GetComponentsInChildren<Image>())
                {
                    item.DOFade(1, 1f);
                }
            }
        }


    }
    public void ClosePanel()
    {
        indexController.InfoPanel.SetActive(false);
    }
    public void BasicFadeOut(GameObject Target, Vector3 TargetOriginPos, NowScreen AfterScreen)
    {
        DOTween.To(() => Target.transform.position, x => Target.transform.position = x, Target.transform.position + new Vector3(2000, 0, 0), 1f).OnComplete(() => { Target.SetActive(false); Target.transform.position = TargetOriginPos; m_NowScreen = AfterScreen; });
        foreach (var item in Target.GetComponentsInChildren<Image>())
        {
            item.DOFade(0, 1f).OnComplete(() => { item.color = new Color(1, 1, 1, 1); });
        }
    }
    public void BasicFadeIn(GameObject Target, Vector3 TargetOriginPos, NowScreen AfterScreen)
    {
        Target.transform.position = Target.transform.position - new Vector3(2000, 0, 0);
        Target.SetActive(true);
        m_NowScreen = AfterScreen;
        DOTween.To(() => Target.transform.position, x => Target.transform.position = x, TargetOriginPos, 1f).OnComplete(() =>
        {
            Target.transform.position = TargetOriginPos;
            if (AfterScreen == NowScreen.MainMenu)
            {
                GachaPanel.SetActive(false);
                BattlePanel.SetActive(false);
                W_and_CPanel.SetActive(false);
            }
        });
        foreach (var item in Target.GetComponentsInChildren<Image>())
        {
            item.DOFade(1, 1f);
        }
    }
    public void ChangeCharacter(int ID)
    {
        if (Data.ThisAccount.HaveCharactorOrNot[ID])
        {
            CharacterBar.sprite = MC_Data.MainCharacterDataList[ID].CharacterBar;
            Data.ThisAccount.NowMainCharactor = ID;
            DataSystem.Save();
        }
    }
    public void Debugger()
    {
        Debug.Log("Bruh");
    }
}

public enum NowScreen
{
    MainMenu,
    ShopMenu,
    Weapon_and_CharacterMenu,
    BattleMenu,
}

