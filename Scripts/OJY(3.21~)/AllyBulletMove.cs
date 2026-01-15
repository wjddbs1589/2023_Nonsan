using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class AllyBulletMove : MonoBehaviour
{
    GameObject mainEnemy;
    public float speed = 10.0f;

    float offsetX;
    float offsetY;
    float offsetZ;

    public LayerMask covorMask;
    private void Awake()
    {
        mainEnemy = GameObject.Find("MainEnemyCar");
        offsetX = Random.Range(-1f,-2f);
        offsetY = Random.Range(-1.5f, -2.5f);
        offsetZ = Random.Range(-3f, 3f);
        StartCoroutine(DestroyCo());
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, mainEnemy.transform.position + new Vector3(offsetX, offsetY, offsetZ), speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, mainEnemy.transform.position + new Vector3(offsetX, offsetY, offsetZ));

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hits;
        if (Physics.Raycast(ray, out hits, 1.0f, covorMask)) //장애물 및 타겟이 아닌 레이어
        {
            var effect = GetImpactEffect(hits.transform.gameObject);
            if (effect == null)
                return;
            var effectIstance = Instantiate(effect, hits.point, new Quaternion()) as GameObject;
            effectIstance.transform.LookAt(hits.point + hits.normal);
            Destroy(effectIstance, 0.3f);
        }


        if (distance >= 0.1)
        {
            transform.LookAt(mainEnemy.transform.position + new Vector3(offsetX, offsetY, offsetZ));
        }
        else
        {
            Destroy(gameObject);
        }     
        

    }
    IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(15.0f);
        Destroy(gameObject);
    }

    //ImpactInfo 클래스의 배열로, 총알 충돌 시 이펙트를 처리할 때 이 ImpactInfo를 참조하여 해당하는 이펙트를 가져옴
    public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    // Unity에서 ScriptableObject, MonoBehaviour 등과 같은 컴포넌트 클래스 내에 다른 클래스를 정의할 때,
    // 해당 클래스를 인스펙터에서 직렬화하여 표시할 수 있도록 지정하는 특성(Attribute)
    [System.Serializable]
    public class ImpactInfo //이 클래스는 총알 충돌 시 이펙트 종류와 충돌한 물체의 재질 종류를 저장
    {
        public LayerType.LayerTypeEnum LayerType;
        public GameObject ImpactEffect;
    }
    //---------------------------------------------------------------------------------------------------
    /// <summary>
    /// 충돌한 물체의 재질 종류를 판별하고, 이펙트 종류를 가져옴
    /// /// 만약 충돌한 물체가 재질을 가지고 있지 않으면, null 값을 반환
    /// </summary>
    /// <param name="impactedGameObject">충돌한 물체</param>
    /// <returns>충돌한 물체의 재질 종류</returns>
    GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var layerType = impactedGameObject.GetComponentInParent<LayerType>();
        if (layerType == null)
            return null;

        foreach (var impactInfo in ImpactElemets)
        {
            if (impactInfo.LayerType == layerType.TypeOfLayer)
            {
                return impactInfo.ImpactEffect;
            }

        }
        return null;
    }
    //---------------------------------------------------------------------------------------------------
}
