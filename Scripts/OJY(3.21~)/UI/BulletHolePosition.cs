using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHolePosition : MonoBehaviour
{
    public RectTransform[] bulletHole;
    void ChangePosition()
    {
        for (int i = 0; i < bulletHole.Length; i++)
        {
            bulletHole[i].anchoredPosition = new Vector2(Random.Range(-1920f, 1920f), Random.Range(-600f, 600f));
        }
    }
}
