using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneChange();
        }
    }    
        
    void SceneChange()
    {
        GameManager.Inst.SceneChanger();
    }

    void OffUI()
    {
        GameManager.Inst.ActiveSetting(false);
    }
}