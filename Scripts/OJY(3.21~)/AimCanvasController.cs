using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCanvasController : MonoBehaviour
{
    //애니메이션 이벤트로 사용될 함수들

    void OnAimCanvas()
    {
        GameManager.Inst.ActiveSetting(true);
    }
    void OffAimCanvas()
    {
        GameManager.Inst.ActiveSetting(false);
    }
    void SceneChanage()
    {
        GameManager.Inst.SceneChanger();
    }
}
