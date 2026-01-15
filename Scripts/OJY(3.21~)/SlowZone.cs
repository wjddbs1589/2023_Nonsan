using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public GameObject mainEnemy;
    float speed = 0;
    public int mapLevel = 0;
    public float slowTime = 3.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(slowCo(other));
        }
    }

    IEnumerator slowCo(Collider other)
    {
        //Time.timeScale = 0.2f;

        other.GetComponent<CinemachineDollyCart>().m_Speed = 2.0f;
        mainEnemy.GetComponent<CinemachineDollyCart>().m_Speed = 2.0f;

        yield return new WaitForSeconds(slowTime);
        Time.timeScale = 1.0f;

        switch (mapLevel)
        {
            case 1:
                speed = 15.0f;
                break;
            case 2:
                speed = 20.0f;
                break;
        }
        other.GetComponent<CinemachineDollyCart>().m_Speed = speed;
        mainEnemy.GetComponent<CinemachineDollyCart>().m_Speed = speed;
    }
}
