using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUISet : MonoBehaviour
{
    public GameObject controller;
    public Vector3 controllerPOS;
    public Vector3 contollerROT;

    private bool GUION = false;
    private bool Stop = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Q))
        {
            GUION = !GUION;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Stop = !Stop;
        }
        if (Stop)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        controller = GameObject.Find("Controller");
        controllerPOS = controller.transform.localPosition;
        contollerROT = new Vector3(controller.transform.localRotation.x, controller.transform.localRotation.y, controller.transform.localRotation.z);
        controller.transform.localPosition = controllerPOS;
        controller.transform.localRotation = Quaternion.Euler(contollerROT);
       
        GUION = false;
    }

    private void OnGUI()
    {
        if (GUION)
        {
            controllerPOS.x = float.Parse(GUI.TextField(new Rect(20, 50, 200, 50), controllerPOS.x.ToString()));
            controllerPOS.y = float.Parse(GUI.TextField(new Rect(20, 100, 200, 50), controllerPOS.y.ToString()));
            controllerPOS.z = float.Parse(GUI.TextField(new Rect(20, 150, 200, 50), controllerPOS.z.ToString()));

            GUI.Label(new Rect(20, 200, 400, 30), " Controller Pos Value : " + controllerPOS.ToString());
            if (GUI.Button(new Rect(20, 250, 400, 30), "Confirm"))
                controller.transform.localPosition = controllerPOS;

            contollerROT.x = float.Parse(GUI.TextField(new Rect(20, 300, 200, 50), contollerROT.x.ToString()));
            contollerROT.y = float.Parse(GUI.TextField(new Rect(20, 350, 200, 50), contollerROT.y.ToString()));
            contollerROT.z = float.Parse(GUI.TextField(new Rect(20, 400, 200, 50), contollerROT.z.ToString()));

            GUI.Label(new Rect(20, 450, 400, 30), " Controller Rot Value : " + contollerROT.ToString());
            if (GUI.Button(new Rect(20, 500, 400, 30), "Confirm"))
                controller.transform.localRotation = Quaternion.Euler(contollerROT);
        }
    }
}
