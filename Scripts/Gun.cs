using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    AudioSource m_AudoiSource;
    public ParticleSystem[] m_MuzzleFlash = new ParticleSystem[4];
    public GameObject m_MuzzleLight;

    private void Start()
    {
        m_AudoiSource = GetComponent<AudioSource>();
        m_AudoiSource.playOnAwake = false;
    }

    public void Fire()
    {
        Debug.Log("น฿ป็");
        StartCoroutine(FireCo());
    }

    public IEnumerator FireCo()
    {
        m_AudoiSource.Play();
        for (int i = 0; i < m_MuzzleFlash.Length; i++)
        {
            m_MuzzleFlash[i].Play();
        }
       
        m_MuzzleLight.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < m_MuzzleFlash.Length; i++)
        {
            m_MuzzleFlash[i].Stop();
        }
        m_MuzzleLight.SetActive(false);
    }


    public void StopFire()
    {
        
    }
}
