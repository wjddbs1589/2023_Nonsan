using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI displayText;
    public float duration = 1f;
    float Score = 0;//800

    float elapsedTime;

    void Awake()
    {
        Score = 0; //기본 보스점수 800
        elapsedTime = 0f;
        displayText = GetComponent<TextMeshProUGUI>();
        // 코루틴 실행
        StartCoroutine(InterpolateNumber());
    }

    IEnumerator InterpolateNumber()
    {
        Score = GameManager.Inst.TotalScore;

        while (elapsedTime < duration)
        {
            // 보간된 숫자 계산
            float currentValue = Mathf.Lerp(0, Score, elapsedTime / duration);

            // UI Text에 표시
            displayText.text = currentValue.ToString("F0"); // 소수점 이하 자리 없이 정수로 표시

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 보간이 완료된 후 최종 값을 표시
        displayText.text = Score.ToString("F0");
    }
}
