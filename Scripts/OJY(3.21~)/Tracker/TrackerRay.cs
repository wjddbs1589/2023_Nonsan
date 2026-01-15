using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerRay : MonoBehaviour
{
    public float m_DefaultLength;
    public GameObject m_Dot;

    

    private void Start()
    {
       
    }
    private void Update()
    {
        PointLine();
        /*
        int layerMask = 1<< LayerMask.NameToLayer("Enemy");
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("input 1");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.transform.root.GetComponent<Enemy>().Die(hit.collider.transform.root.GetComponent<Enemy>());
                    Debug.Log("enemy");
                }
            }

        }
        */

    }
    private void PointLine()
    {
        
        float target = m_DefaultLength;

        RaycastHit hit = CreateRay(target);

        Vector3 endPos = transform.position + (transform.forward * target);

        if (hit.collider != null)
            endPos = hit.point;

        m_Dot.transform.position = endPos;

        
    }
    private RaycastHit CreateRay(float length)
    {
        int layerMask = 1 << LayerMask.NameToLayer("JC");
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        
        Physics.Raycast(ray, out hit, m_DefaultLength, layerMask);
        return hit;


    }

   
}
