using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyFix : MonoBehaviour
{
    GameObject enemy;
    Vector3 lookRotation;
    Vector3 Origin;
    public bool lookBack; //보고있는 방향 판별용 true면 뒤, false면 앞을 보고있음
    private void Awake()
    {
        enemy = transform.GetChild(0).gameObject;
        if (lookBack)
        {
            Origin = new Vector3(0.034f, 0.722f, -0.627f);
        }
        
    }
    private void Update()
    {
        Origin = transform.position;
        enemy.transform.position = Origin;
    }
}
