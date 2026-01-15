using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStop : MonoBehaviour
{
    bool battle = false;
    public float battleTime = 4.0f;
    public int mapLevel = 1;
    float speed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && battle == false)
        {
            battle = true;
            StartCoroutine(battleCo(other));
        }
    }
    IEnumerator battleCo(Collider other)
    {
        other.transform.GetComponent<CinemachineDollyCart>().m_Speed = 0.0f;
        yield return new WaitForSeconds(battleTime);
        switch(mapLevel)
        {
            case 1:
                speed = 15.0f;
                break;
            case 2:
                speed = 20.0f;
                break;
        }
        other.transform.GetComponent<CinemachineDollyCart>().m_Speed = speed;
    }
}
