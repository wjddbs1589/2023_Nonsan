using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySmoke : MonoBehaviour
{
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    public Transform smokePosition;
    public void SmokeActive()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform);
        explosion.transform.parent = smokePosition.transform;

        GameObject smoke = Instantiate(smokePrefab, transform);
        smoke.transform.parent = smokePosition.transform;

        transform.gameObject.SetActive(false);
    }
}
