using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePointer : MonoBehaviour
{
	public Transform pointerImg;

	//public Camera camera;

	private Transform target;

	// Use this for initialization

	void Start()
	{

		target = GetComponent<Transform>();

	}



	// Update is called once per frame

	void Update()
	{

		//Vector3 screenPos = camera.WorldToScreenPoint(this.transform.position);
		//pointerImg.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
	}
}
