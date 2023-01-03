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

    void Start()
    {
        MainPanelStartPos = MainPanel.transform.position;
        BuyStartPos = BuyPanel.transform.position;
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

}

public enum NowScreen
{
    MainMenu,
    ShopMenu,
    Weapon_and_CharacterMenu,
    BattleMenu,
}

