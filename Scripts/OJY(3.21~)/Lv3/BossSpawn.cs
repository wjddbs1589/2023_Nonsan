using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Victim;
    Transform spawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
    }
    private void Awake()
    {
        spawnPosition = transform.transform;
    }
    void spawnBoss()
    {
        GameObject boss = Instantiate(Boss);
        boss.transform.position = spawnPosition.transform.position;

        GameObject victim = Instantiate(Victim);
        victim.transform.position = boss.transform.position + new Vector3(0,0,0.88f);
    }
}
