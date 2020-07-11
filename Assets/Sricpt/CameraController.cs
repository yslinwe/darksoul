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
	public bool IsAI;
	private GameObject PlayHandler;
	private GameObject CameraHandler;
	private GameObject maincamera;
	private GameObject model;
	private LockTarget lockTarget;
	// Use this for initialization
	private float tempEulerAnglex;
	private Vector3 currentVelocity;
	void Start () {
		CameraHandler = transform.parent.gameObject;
		PlayHandler = CameraHandler.transform.parent.gameObject;
		ActorControl ac = PlayHandler.GetComponent<ActorControl>();
		model = ac.model;
		pi = ac.pi;
		tempEulerAnglex = 20.0f;
		if(!IsAI)
		{
			maincamera = Camera.main.gameObject;
			Cursor.lockState = CursorLockMode.Locked;
			lockDot.enabled = false;
		}
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
		if(!IsAI)
		{
			maincamera.transform.position = Vector3.SmoothDamp(maincamera.transform.position , transform.position ,ref currentVelocity ,cameraSmoothDamp);
		//	maincamera.transform.eulerAngles = transform.eulerAngles;
			maincamera.transform.LookAt(transform.position);
		}
	}
	void Update()
	{
		if(lockTarget != null)
		{
			if(!IsAI)
				lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0.0f,lockTarget.halfHeight,0.0f));
			if(Vector3.Distance(lockTarget.obj.transform.position,model.transform.position)>10.0f)
			{
				lockSetting(null,false,false,IsAI);
			}
			if(lockTarget.am !=null && lockTarget.am.sm.isDie)
			{
				lockSetting(null,false,false,IsAI);
			}
		}
	}
	public void LockUnlock()
	{
		
		Vector3 modelOrigin = model.transform.position + new Vector3(0,1,0);
		Vector3 boxCenter = modelOrigin + model.transform.forward * 5.0f;
		Collider[] cols = Physics.OverlapBox(boxCenter,new Vector3(0.5f,0.5f,5f),
		model.transform.rotation,LayerMask.GetMask("Enemy"));

		if(cols.Length == 0)
		{
			lockSetting(null,false,false,IsAI);
		}
		else
		{
			foreach(var col in cols)
			{
				if(lockTarget != null&&lockTarget.obj == col.gameObject)
				{
					lockSetting(null,false,false,IsAI);
					break;
				}
				lockTarget = new LockTarget(col.gameObject,col.bounds.extents.y);
				if(lockTarget.am !=null && lockTarget.am.sm.isDie)
				{
					lockSetting(null,false,false,IsAI);
				}
				else
				{
					lockSetting(lockTarget,true,true,IsAI);
				}
				break;
			}
		}
	}
	private void lockSetting(LockTarget _lockTarget,bool _lockDotEnable,bool _lockState,bool IsAI)
	{
		lockTarget = _lockTarget;
		if(!IsAI)
		{
			lockDot.enabled = _lockDotEnable;
		}
		lockState = _lockState;
	}
	private class LockTarget
	{
		public GameObject obj;
		public float halfHeight;
		public ActorManager am;
		public LockTarget(GameObject _obj,float _halfHeight)
		{
			obj = _obj;
			halfHeight = _halfHeight;
			am = obj.GetComponent<ActorManager>();
		}
	}
}
