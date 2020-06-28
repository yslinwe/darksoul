using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
	public IUserInput pi;
	public float HorizontalSpeed = 100.0f;
	public float VerticalSpeed = 100.0f;
	public float upMaxAngle = -15.0f;
	public float downMaxAngle = 30.0f; 
	public float cameraSmoothDamp = 0.05f;
	public Image lockDot;
	public bool lockState;
	private GameObject PlayHandler;
	private GameObject CameraHandler;
	private GameObject camera;
	private GameObject model;
	private LockTarget lockTarget;
	// Use this for initialization
	private float tempEulerAnglex;
	private Vector3 currentVelocity;
	void Awake () {
		CameraHandler = transform.parent.gameObject;
		PlayHandler = CameraHandler.transform.parent.gameObject;
		model = PlayHandler.GetComponent<ActorControl>().model;
		tempEulerAnglex = 20.0f;
		camera = Camera.main.gameObject;
		Cursor.lockState = CursorLockMode.Locked;
		lockDot.enabled = false;
		lockState = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(lockTarget == null)
		{
			Vector3 tempModelAngle = model.transform.eulerAngles;
			//左右
			PlayHandler.transform.Rotate(Vector3.up,HorizontalSpeed*pi.Jright*Time.fixedDeltaTime);
			//上下
			tempEulerAnglex -= VerticalSpeed*pi.Jup*Time.fixedDeltaTime;
			tempEulerAnglex = Mathf.Clamp(tempEulerAnglex,upMaxAngle,downMaxAngle);//限制值的变化
			CameraHandler.transform.localEulerAngles = new Vector3(tempEulerAnglex,0.0f,0.0f);

			model.transform.eulerAngles = tempModelAngle;
		}
		else
		{
			Vector3 tempModelforward = lockTarget.obj.transform.position - model.transform.position;
			tempModelforward.y = 0.0f;
			PlayHandler.transform.forward = tempModelforward;
			CameraHandler.transform.LookAt(lockTarget.obj.transform.position);
		}

		camera.transform.position = Vector3.SmoothDamp(camera.transform.position , transform.position ,ref currentVelocity ,cameraSmoothDamp);
		//camera.transform.eulerAngles = transform.eulerAngles;
		camera.transform.LookAt(CameraHandler.transform);
	}
	void Update()
	{
		if(lockTarget != null)
		{
			lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0.0f,lockTarget.halfHeight,0.0f));
			if(Vector3.Distance(lockTarget.obj.transform.position,model.transform.position)>10.0f)
			{
				lockTarget = null;
				lockDot.enabled = false;
				lockState = false;
			}
		}
	}
	public void LockUnlock()
	{
		
		Vector3 modelOrigin = model.transform.position + new Vector3(0,1,0);
		Vector3 boxCenter = modelOrigin + model.transform.forward * 5.0f;
		Collider[] cols = Physics.OverlapBox(boxCenter,new Vector3(0.5f,0.5f,5f),
		model.transform.rotation,LayerMask.GetMask("Enmey"));

		if(cols.Length == 0)
		{
			lockTarget = null;
			lockDot.enabled = false;
			lockState = false;
		}
		else
		{
			foreach(var col in cols)
			{
				if(lockTarget != null&&lockTarget.obj == col.gameObject)
				{
					lockTarget = null;
					lockDot.enabled = false;
					lockState = false;
					break;
				}
				lockTarget = new LockTarget(col.gameObject,col.bounds.extents.y);
				lockDot.enabled = true;
				lockState = true;
				break;
			}
		}
	}
	private class LockTarget
	{
		public GameObject obj;
		public float halfHeight;
		public LockTarget(GameObject _obj,float _halfHeight)
		{
			obj = _obj;
			halfHeight = _halfHeight;
		}
	}
}
