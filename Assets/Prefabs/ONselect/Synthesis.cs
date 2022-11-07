using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Synthesis : MonoBehaviour
{
    //public ParticleSystem BURST;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Button()
    {
        transform.DOPunchScale(new Vector3(0.08f, 0.08f, 0.08f), 0.1f).SetEase(Ease.InOutQuint);

    }
    //public void burst()
    //{
    //    BURST.Play();

    //}
}
