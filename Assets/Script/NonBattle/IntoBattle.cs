using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoBattle : MonoBehaviour
{
    public StageInfoSO stageInfoSO;
    public MainPlayer m_mainplayer;
    public void ChangeScene(int id)
    {
        m_mainplayer.ThisRound_MainCharacter_ID = id;
        SceneManager.LoadScene(1);
    }
   
}
