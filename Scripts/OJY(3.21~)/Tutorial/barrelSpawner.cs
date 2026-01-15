using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrelSpawner : MonoBehaviour
{
    public GameObject explosionPrefab;
    GameObject barrel;
    private void Awake()
    {
        barrel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (barrel == null)
        {
            StartCoroutine(RespawnBarrel());            
        }
    }
    IEnumerator RespawnBarrel()
    {
        barrel = Instantiate(explosionPrefab);
        yield return new WaitForSeconds(2f);
        barrel.transform.position = transform.position;
    }
}
