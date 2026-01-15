using EnemyAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Lv3BossCutScenePlayer : MonoBehaviour
{
    [SerializeField] GameObject Timeline;
    [SerializeField] bool cutSceneStart = false;           //타임라인 시작 여부

    //---------------------------------------
    /// <summary>
    /// cutSceneStart의 값이 변경될때 함수 실행하기 위해 추가함
    /// </summary>
    public bool CutSceneStart
    {
        get => cutSceneStart;
        set
        {
            cutSceneStart = value;
            cutSceneStarted?.Invoke();
        }
    }
    public System.Action cutSceneStarted;
    //---------------------------------------

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(timeLineCo());
        }
    }
    /// <summary>
    /// 타임라인 실행 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator timeLineCo()
    {
        yield return new WaitForSeconds(2f);
        CutSceneStart = true;
        Timeline.SetActive(true);
        Timeline.transform.GetChild(0).GetComponent<PlayableDirector>().Play();        
    }
}
