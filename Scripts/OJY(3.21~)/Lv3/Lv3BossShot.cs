using AmplifyImpostors;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lv3BossShot : MonoBehaviour
{
    public float ShotAbleTime;
    //플레이어 수만큼 사격 활성화
    IEnumerator ActiveAimCo()
    {
        GameManager.Inst.ActiveSetting(true);
        yield return new WaitForSeconds(ShotAbleTime);
        GameManager.Inst.ActiveSetting(false);
    }

}
