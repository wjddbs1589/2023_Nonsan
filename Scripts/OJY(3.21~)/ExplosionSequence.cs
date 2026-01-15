using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSequence : MonoBehaviour
{
    public GameObject[] explosionPrefabs = new GameObject[3];
    public GameObject firePrefab;
    Transform[] explosionPosition = new Transform[6];


    private void Awake()
    {
        for (int i = 0;i<explosionPosition.Length; i++)
        {
            explosionPosition[i] = transform.GetChild(i);
        }        
    }
    private void Start()
    {
        StartCoroutine(ExplosionCo());
    }
    IEnumerator ExplosionCo()
    {
        for (int i = 0; i < explosionPosition.Length; i++)
        {
            int randNum = Random.Range(0, 3);
            Instantiate(explosionPrefabs[randNum], explosionPosition[i]);
            Instantiate(firePrefab, explosionPosition[i]);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
