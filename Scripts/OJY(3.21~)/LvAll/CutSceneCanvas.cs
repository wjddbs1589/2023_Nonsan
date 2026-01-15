using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutSceneCanvas : MonoBehaviour
{
    Vector3 originPos;
    [Tooltip("1이면 위로 -1이면 아래로 이동")]
    public float UpDownCheck = 1;
    float distance;
    float moveDistance = 200f;
    [HideInInspector]public float time = 3.0f;
    bool cutSceneStart = false;
    private void Start()
    {
        originPos = transform.position;
    }

    public void cutSceneEffect()
    {
        if (!cutSceneStart)
        {
            StartCoroutine(EndCo());
        }
    }

    IEnumerator EndCo()
    {
        cutSceneStart = true;
        Time.timeScale = 1f;
        if (GameManager.Inst != null)
        {
            //캔버스 및 사격 비활성화
            GameManager.Inst.ActiveSetting(false);
        }

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos + Vector3.up * moveDistance * UpDownCheck, 200 * Time.deltaTime);
            distance = Vector3.Distance(transform.position, originPos);
            if (distance >= 200.0f)
            {
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(time);


        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, 200 * Time.deltaTime);
            distance = Vector3.Distance(transform.position, originPos);
            if (distance <= 0)
            {
                if (GameManager.Inst != null)
                {
                    // 캔버스 및 사격 활성화
                    GameManager.Inst.ActiveSetting(true);
                }
                break;
            }
            yield return null;
        }

    }
}
