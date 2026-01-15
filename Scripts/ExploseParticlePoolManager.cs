using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploseParticlePoolManager : MonoBehaviour
{
    //파티클 풀
    public List<ExploseParticleManager> _ExploseParticles = new List<ExploseParticleManager>();

    //풀에서 사용가능한 파티클을 반환함
    public ExploseParticleManager GetUseableExploseParticle()
    {
        for(int i = 0; i< _ExploseParticles.Count; i++)
        {
            if(!_ExploseParticles[i].gameObject.activeInHierarchy)
            {
                return _ExploseParticles[i];
            }
        }
        return null;
    }
}
