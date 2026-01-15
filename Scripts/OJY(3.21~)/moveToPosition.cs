using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class moveToPosition : MonoBehaviour
{
    public Transform[] movePosition;
    public GameObject player;
    int moveTargetNum = 0;
    Vector3 distance;

    private void Update()
    {
        if (moveTargetNum != movePosition.Length)
        {
            distance = movePosition[moveTargetNum].transform.position - transform.position;
            if (distance == Vector3.zero)
            {
                moveTargetNum++;
            }
            transform.position = Vector3.MoveTowards(transform.position, movePosition[moveTargetNum].position, 1);
            transform.DOLookAt(player.transform.position, 1f);
        }
        else
        {
            Destroy(gameObject, 2.0f);
        }
    }
}
