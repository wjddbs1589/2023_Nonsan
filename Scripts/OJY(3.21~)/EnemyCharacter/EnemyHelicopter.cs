using Cinemachine;
using EnemyAI;
using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHelicopter : MonoBehaviour
{
    CinemachineDollyCart cart;
    [SerializeField] GameObject SmokePrefab;
    [SerializeField] GameObject firePrefab;
    [SerializeField] Transform smokePosition;
    [SerializeField] GameObject SmallExplosionPrefab;
    [SerializeField] GameObject explosionPrefab;
    public bool dead = false;
    public LayerMask enemyMask;
    private void Awake()
    {
        dead = false;
        cart = transform.parent.GetComponent<CinemachineDollyCart>();
    }

    /// <summary>
    /// 애니메이션에서 실행될 이벤트 함수
    /// </summary>
    void SmallExplosion()
    {
        if (Application.isPlaying)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform);
            explosion.transform.parent = null;

            explosion.transform.localScale *= 2;
            GameObject smoke = Instantiate(SmokePrefab,explosion.transform);
            smoke.transform.position = transform.position + transform.up * 2;   

            GameObject fire = Instantiate(firePrefab, transform);
            fire.transform.position = transform.position + transform.up * 2;
            fire.transform.localScale *= 2;
        }
    }
    /// <summary>
    /// 애니메이션에서 실행될 이벤트 함수 
    /// </summary>
    void DestroyExplosion()
    {
        if (Application.isPlaying)
        {
            cart.enabled = false;
            dead = true;

            GameObject explosion = Instantiate(explosionPrefab, transform);
            explosion.transform.localScale *= 10;

            Destroy(gameObject, 10.0f);
        }
    }
    void DeadChanger()
    {
        dead = true;
        AudioSource sound = GetComponent<AudioSource>();
        sound.enabled = true;
    }
}
