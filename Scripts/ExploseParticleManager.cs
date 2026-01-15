using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploseParticleManager : MonoBehaviour
{
    public ParticleSystem[] _BombParticleSystems = new ParticleSystem[3];

    private void OnEnable()
    {
        StartCoroutine(BombParticleTurnOff());
    }

    IEnumerator BombParticleTurnOff()
    {
        yield return new WaitUntil(() => _BombParticleSystems[0].isStopped);
        yield return new WaitUntil(() => _BombParticleSystems[1].isStopped);
        yield return new WaitUntil(() => _BombParticleSystems[2].isStopped);
        gameObject.SetActive(false);
    }
}
