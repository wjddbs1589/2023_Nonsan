using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lv3BossWithSmoke : MonoBehaviour
{
    public GameObject smoke;   
    public GameObject Boss;
    public GameObject Victim;
    public AudioClip smokeSound;

    Transform spawnPosition;

    GameObject boss;
    Animator bossAnim;
    GameObject victim;
    Animator victimAnim;

    private void Awake()
    {
        spawnPosition = GameObject.Find("SmokeStopArea").transform;
    }

    IEnumerator SpawnBossCo()
    {
        if (Application.isPlaying)
        {
            GameObject obj = Instantiate(smoke);
            AudioSource.PlayClipAtPoint(smokeSound, transform.position, 0.5f);
            obj.transform.position = spawnPosition.transform.position;
            yield return new WaitForSeconds(1.5f);
            SpawnBoss();
        }
    }
    void SpawnBoss()
    {
        boss = Instantiate(Boss);
        boss.transform.position = spawnPosition.transform.position;

        victim = Instantiate(Victim);
        victim.transform.position = boss.transform.position + new Vector3(0, 0, 0.88f);

        bossAnim = boss.GetComponent<Animator>();
        victimAnim = victim.GetComponent<Animator>();
    }
    void AnimationPlay()
    {
        bossAnim.SetTrigger("Dead");        
        victimAnim.SetTrigger("Release");
    }

    void TimeRecovery()
    {
        Time.timeScale = 1;
    }
}
