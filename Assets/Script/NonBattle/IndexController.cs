using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class IndexController : MonoBehaviour
{
    public Sprite[] w_Cha_top = new Sprite[2];
    public Image IndexTopBar;
    public GameObject IndexContent;
    public int NowFoucsWeapon = 0;
    public Image[] cards = new Image[5];   
    public GameObject WeaponPrefab;
    public HorizontalLayoutGroup horizontal;
    public Sprite[] cardORI = new Sprite[5];
    public GameObject InfoPanel;
    private void Start()
    {
        
    }
    public void CardInstall()
    {
        for (int i = 0; i < cardORI.Length; i++)
        {
            cardORI[i] = cards[i].sprite;
        }
    }
}
