using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOfMainEnemy : MonoBehaviour
{
    [SerializeField] CinemachineDollyCart cart;
    CinemachineSmoothPath path;
    public bool right;
    float rightCheck = 1;
    float rotateAmount = 720;
    private void Awake()
    {
        path = cart.m_Path.GetComponent<CinemachineSmoothPath>();
    }

    private void Start()
    {
        if (!right)
        {
            rightCheck *= -1;
        }
    }
    private void Update()
    {        
        if ((path.PathLength - cart.m_Position) / path.PathLength > 0f)
        {
            transform.Rotate(new Vector3(0, 0, rotateAmount * Time.deltaTime * rightCheck));
        }
    }
    
}
