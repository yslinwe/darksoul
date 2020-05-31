using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public InputControl pi;
	public float HorizontalSpeed = 100.0f;
	public float VerticalSpeed = 100.0f;
	public float upMaxAngle = -15.0f;
	public float downMaxAngle = 30.0f; 
	public float cameraSmoothDamp = 0.05f;
	private GameObject PlayHandler;
	private GameObject CameraHandler;
	private GameObject playerCamera;
	private GameObject model;
	// Use this for initialization
	private float tempEulerAnglex;
	private Vector3 currentVelocity;
	void Awake () {
		CameraHandler = transform.parent.gameObject;
		PlayHandler = CameraHandler.transform.parent.gameObject;
		model = PlayHandler.GetComponent<ActorControl>().model;
		tempEulerAnglex = 10.0f;
		playerCamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 tempModelAngle = model.transform.eulerAngles;
		//左右
		PlayHandler.transform.Rotate(Vector3.up,HorizontalSpeed*pi.Jright*Time.fixedDeltaTime);
		//上下
		tempEulerAnglex -= VerticalSpeed*pi.Jup*Time.fixedDeltaTime;
		tempEulerAnglex = Mathf.Clamp(tempEulerAnglex,upMaxAngle,downMaxAngle);
		CameraHandler.transform.localEulerAngles = new Vector3(tempEulerAnglex,0.0f,0.0f);

		model.transform.eulerAngles = tempModelAngle;

		playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position , transform.position ,ref currentVelocity ,cameraSmoothDamp);
		playerCamera.transform.eulerAngles = transform.eulerAngles;
	}
}
