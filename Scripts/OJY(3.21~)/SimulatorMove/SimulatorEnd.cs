using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Inst.ActiveSetting(false);
            GameManager.Inst.SceneStart = false;
            GameManager.Inst.udpSender.Send();

            //transform.GetChild(0).gameObject.SetActive(true);

            //Destroy(gameObject);
        }
    }
}
