using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    TextMeshProUGUI text;
    string min;
    string sec;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator TimerCo(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 15)
            {
                text.color = Color.red;
            }
            else
            {
                text.color = Color.white;
            }

            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            min = minutes.ToString("D2");
            sec = seconds.ToString("D2");
            text.text = $"{min} : {sec}";
            yield return null;
        }

        text.text = "00 : 00"; 
    }
}
