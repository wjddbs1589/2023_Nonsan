using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TargetHuman : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {        
        anim = GetComponent<Animator>();
    }
    public void Hit()
    {
        anim.SetBool("Down", true);
        StartCoroutine(SetFalse());
    }
    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Down", false);
    }
}
