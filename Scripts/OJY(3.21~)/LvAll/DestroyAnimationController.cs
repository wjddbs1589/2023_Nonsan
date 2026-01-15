using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimationController : MonoBehaviour
{
    Animator anim;              
    public GameObject crashSmoke;
    AudioClip audioClip;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        audioClip = GetComponent<AudioSource>().clip;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetComponent<Collider>().enabled = false;
            anim.enabled = true;
            if (crashSmoke)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
                GameObject smoke = Instantiate(crashSmoke, transform);
                smoke.transform.localScale = Vector3.one * 2;
            }
        }
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
