using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoBattle : MonoBehaviour
{
    public StageInfoSO stageInfoSO;
    public MainPlayer m_mainplayer;
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
