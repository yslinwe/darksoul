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
	private GameObject camera;
	private GameObject model;
	// Use this for initialization
	private float tempEulerAnglex;
	private Vector3 currentVelocity;
	void Awake () {
		CameraHandler = transform.parent.gameObject;
		PlayHandler = CameraHandler.transform.parent.gameObject;
		model = PlayHandler.GetComponent<ActorControl>().model;
		tempEulerAnglex = 10.0f;
		camera = Camera.main.gameObject;
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

		camera.transform.position = Vector3.SmoothDamp(camera.transform.position , transform.position ,ref currentVelocity ,cameraSmoothDamp);
		camera.transform.eulerAngles = transform.eulerAngles;
	}
}
