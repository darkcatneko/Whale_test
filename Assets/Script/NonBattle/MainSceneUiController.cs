using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainSceneUiController : MonoBehaviour
{
    public PlayerData Data;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI GemText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = Data.ThisAccount.CoinCount.ToString();
        GemText.text = Data.ThisAccount.GemCount.ToString();
    }
}
