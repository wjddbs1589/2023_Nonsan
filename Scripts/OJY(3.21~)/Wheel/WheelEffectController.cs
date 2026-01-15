using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelEffectController : MonoBehaviour
{
    [Tooltip("바퀴에 포함된 파티클시스템")]
    public ParticleSystem[] wheelEffect = new ParticleSystem[4];

    Lv3GuardMoveStart moveStart;
    private void Awake()
    {
        moveStart = GameObject.Find("part4GuardStarter").GetComponent<Lv3GuardMoveStart>();
    }

    //차량의 이속이 바뀌는것을 감지하여 함수 실행
    private void Start()
    {
        moveStart.StartEffect += effectStart;
    }
    /// <summary>
    /// 바퀴에 이펙트 재생
    /// </summary>
    void effectStart()
    {
        foreach (var effect in wheelEffect) 
        {
            effect.Play();
        }
    }

}
