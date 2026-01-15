using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObject : MonoBehaviour
{
    GameObject[] obj;

    private void Awake()
    {
        obj = new GameObject[4];
        for (int i = 0;i<4;i++)
        {
            obj[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < GameManager.Inst.playerCount; i++)
        {
            obj[i].SetActive(true);
        }
    }
}
