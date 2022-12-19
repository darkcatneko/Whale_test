using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBattleUI : MonoBehaviour
{
    [SerializeField] bool GameIIsPaused = false; 
    public void  OnSettingButtonClicked()
    {
        if (GameIIsPaused == true)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    void Resume()
    {
        Time.timeScale = 1f; GameIIsPaused = false;
    }
    void Pause()
    {
        Time.timeScale = 0f;GameIIsPaused = true;
    }
}
