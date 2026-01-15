using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerKillScore : MonoBehaviour
{
    TextMeshProUGUI displayText;
    float duration = 0.5f;
    float killCount = 0;

    float elapsedTime;

    public int playerNum;

    void Awake()
    {
        elapsedTime = 0f;
        displayText = GetComponent<TextMeshProUGUI>();
        // 코루틴 실행
        StartCoroutine(InterpolateNumber());
    }

    IEnumerator InterpolateNumber()
    {
        killCount = GameManager.Inst.killCount[playerNum];

        while (elapsedTime < duration)
        {
            // 보간된 숫자 계산
            float currentValue = Mathf.Lerp(0, killCount, elapsedTime / duration);

            // UI Text에 표시
            displayText.text = currentValue.ToString("F0"); // 소수점 이하 자리 없이 정수로 표시

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 보간이 완료된 후 최종 값을 표시
        displayText.text = killCount.ToString("F0");
    }
}
