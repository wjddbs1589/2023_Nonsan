using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    [SerializeField]
    //private Transform startPosition;
    //private Transform TargetPositon; //location1
    //private GameObject enemy;

    private GameObject spawnPoint;

    public GameObject tester;
    public GameObject target;
    public Rigidbody rigid;
    public GameObject testerSpawnArea;

    // Start is called before the first frame update
    void Start()
    {
        enemyCreate();
        spawnPoint.GetComponent<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //enemyCreate();

        if (transform.position != target.transform.position)
        {
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + forPos.normalized, Time.deltaTime * PlayerSpeed);
            //myRigid.MovePosition(transform.position + forPos.normalized * Time.deltaTime * PlayerSpeed);

            transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, Time.deltaTime);
            rigid.MovePosition(transform.position + target.transform.position * Time.deltaTime);
            //각 포지션에 할당해줘야함
        }
    }

    private void Update()
    {
        //Debug.Log(spawnPoint.transform.GetChild(0).name);
        //enemyCreate();
    }

    void enemyCreate()
    {
        for (int i = 1; i <= 3; i++)
        {
            Instantiate(tester);
        }

    }
}
