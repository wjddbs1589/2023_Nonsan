using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenePlayer : MonoBehaviour
{
    public Canvas canvas;
    public CutSceneCanvas[] CS_canvas = new CutSceneCanvas[2];
    public float PlayTime = 3.0f;
    public bool played = false;
    private void Awake()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("CutSceneCanvas").GetComponent<Canvas>();
        }
        for(int i = 0; i < 2; i++)
        {
            CS_canvas[i]=canvas.transform.GetChild(i).GetComponent<CutSceneCanvas>();
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (CutSceneCanvas SceneCanvas in CS_canvas)
            {
                Time.timeScale = 1f;
                SceneCanvas.time = PlayTime;
                SceneCanvas.cutSceneEffect();
            }
            if (!played)
            {
                played = true;
            }            
        }
    }

}
