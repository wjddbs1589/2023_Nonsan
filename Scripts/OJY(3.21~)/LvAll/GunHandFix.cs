using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandFix : MonoBehaviour
{
    [SerializeField] GameObject[] bodyParts = new GameObject[2];
    Transform[] bodyPosition = new Transform[2];
    private void Awake()
    {
        for(int i = 0; i < bodyPosition.Length; i++)
        {
            bodyPosition[i]=transform.GetChild(i).transform;
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].transform.position = bodyPosition[i].position;
        }
    }
}
