using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackerInput1 : MonoBehaviour
{

    public SteamVR_Input_Sources inputSource;
    public SteamVR_Action_Boolean actionBoolean;


    // Update is called once per frame
    void Update()
    {

        if (SteamVR.initializedState != SteamVR.InitializedStates.InitializeSuccess)
        {
            return;
        }


        if (actionBoolean.GetStateDown(inputSource))
        {
            Debug.Log("On2");

        }

    }


}
