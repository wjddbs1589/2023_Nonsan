using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPathGizmo : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private void OnDrawGizmos()
    {
        // 노란색 선을 그립니다.
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
