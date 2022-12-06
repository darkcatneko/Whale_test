using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class test : MonoBehaviour
{
    public PlayableDirector TimeLine;
   public void PlayTest()
    {
        TimeLine.Play();
    }
}
