using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Destory : MonoBehaviour
{
    public Material fuck;
    public float clip;
    public ParticleSystem burst;
    GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        fuck.SetFloat("_Clip", clip);
    }

    public void Buttontest()
    {

        DOTween.To(() => clip, x => clip = x, 1f, 1f).SetEase(Ease.OutQuart);

        //transform.DORotate(new Vector3(0, 360, 0), 2, RotateMode.FastBeyond360).SetEase(Ease.InOutQuint);
        //transform.DOMove(new Vector3(0,1f, 0), 2).SetEase(Ease.InOutQuint);

        //yield return new WaitForSeconds(0.5f);

        //transform.DOPunchScale(new Vector3(0.05f,0.05f,0.05f),0.1f).SetEase(Ease.InOutQuint);

    }

    public void BURST()
    {
        burst.Play();
    }
    public void recover()
    {
        DOTween.To(() => clip, x => clip = x, 0f, 1f).SetEase(Ease.OutQuart);
    }
}
