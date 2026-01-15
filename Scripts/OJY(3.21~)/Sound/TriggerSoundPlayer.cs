using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SoundPlayer());
        }
    }

    IEnumerator SoundPlayer()
    {
        audioSource.Play();
        yield return new WaitForSeconds(5f);
        Destroy(this);
    }
}
