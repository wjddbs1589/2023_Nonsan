using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward * 1080.0f * Time.deltaTime);
    }
}
