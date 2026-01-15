using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorStart : MonoBehaviour
{
    //출발지의 트리거에 들어갔을때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Inst.ActiveSetting(true);
            GameManager.Inst.SceneStart = true;//씬의 시작 지점에 도달하였음
            GameManager.Inst.udpSender.Send(); //정보 전달
        }
    }
}
