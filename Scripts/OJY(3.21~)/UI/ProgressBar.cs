using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    Slider slider;
    private float targetValue;
    private float duration = 60.0f;

    public TimerText timerText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public IEnumerator TimerStart(int level)
    {
        switch (level)
        {
            case 1:
                slider.value = 0;
                targetValue = 0.33f;
                duration = 78;
                break;
            case 2:
                slider.value = targetValue;
                targetValue = 0.67f;
                duration = 55;
                break;
            case 3:
                slider.value = targetValue;
                targetValue = 1.0f;
                duration = 60;
                break;
            default:
                break;
        }
        StartCoroutine(timerText.TimerCo(90));

        float elapsedTime = 0f; // 경과 시간
        float startValue = slider.value; // 시작 값

        while (elapsedTime < duration)
        {
            yield return null;

            elapsedTime += Time.deltaTime;

            // 보간된 값 계산
            float t = Mathf.Clamp01(elapsedTime / duration); // 0부터 1까지의 값으로 정규화
            float interpolatedValue = Mathf.Lerp(startValue, targetValue, t);

            slider.value = interpolatedValue;

            if (slider.value >= targetValue)
            {
                break;
            }
        }
    }
}
