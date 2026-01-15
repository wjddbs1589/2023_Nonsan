using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimPrefabInfo : MonoBehaviour
{
    public GameObject[] Aim = new GameObject[4];
    
    public GameObject[] ReturnAim()
    {
        return Aim;
    }
}
