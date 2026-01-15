using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class shotController : MonoBehaviour
{
    Canvas canvas;
    public void AimSetting()
    {
        canvas = GetComponent<Canvas>();
        canvas.planeDistance = 20;
        GameManager.Inst.ActiveSetting(false);
    }
}
