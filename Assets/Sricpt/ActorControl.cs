using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ActorControl : MonoBehaviour {
	public GameObject model;
	public CameraController camCon;
	public IUserInput pi;
	public float walkSpeed = 2.0f; 
	public float runSpeed = 2.8f;
	public float jumpVelocity = 0.0f;
	public float rollVelocity = 3.0f;
	[Space(10)]
	[Header("===== friction settings =====")] //地面物体要设layer 为ground 
	public PhysicMaterial frictionOne;
	public PhysicMaterial frictionZero;
	private float targetLerp;
	private Animator anim;
	//[SerializeField]
	private Rigidbody rigid;
	private Vector3 movingvec;
	private Vector3 trustvec;
	private Vector3 deltaPos;
	private bool lockPlaner = false;
	private bool trackDirection = false;
	private bool isNotJump = false;
	private CapsuleCollider col;
	// Use this for initialization
	void Awake () {
		IUserInput [] inputs  = GetComponents<IUserInput>(); //多种输入方式
		foreach(var input in inputs)
		{
			if(input.enabled == true)
				pi = input;
				break;
		}
		anim  = model.GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		col   = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update ()//Time.deltaTime 1/60
	{	
		if (pi.lockon)
		{
			camCon.LockUnlock();
		}
		float TargetRunMutil = pi.run?2.0f:1.0f;//walk转run
		if(camCon.lockState == false)
		{
			anim.SetFloat("forward",pi.Dmag*Mathf.Lerp(anim.GetFloat("forward"),TargetRunMutil,0.4f));
			anim.SetFloat("right",0);
		}
		else
		{
			Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
			anim.SetFloat("forward",localDvec.z * TargetRunMutil);
			anim.SetFloat("right",  localDvec.x * TargetRunMutil);
		}
		anim.SetBool("defense",pi.defense);
		if(pi.roll||rigid.velocity.magnitude>7.0f)
			anim.SetTrigger("roll");
		
		if(pi.jump)
		{
			anim.SetTrigger("jump");
			isNotJump = false;
		}
		if(pi.attach&&checkeState("ground")&&isNotJump&&anim.IsInTransition(0)==false)
			anim.SetTrigger("attach");

		if(camCon.lockState == false)
		{
			if(pi.Dmag>0.1f)//模为0时，pi.Dvec将为(0,0,0)，forword将变回初始值,>0.1f是因为pi里面的Dright和Dup不一定为0
				model.transform.forward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.8f);
			
			if(lockPlaner==false)
				movingvec = model.transform.forward * pi.Dmag * walkSpeed*(pi.run?runSpeed:1.0f);
			}
		else
		{
			if(lockPlaner == false)
				movingvec = pi.Dvec * pi.Dmag * walkSpeed*(pi.run?runSpeed:1.0f);
			if(trackDirection == false)
				model.transform.forward = transform.forward;
			else
				model.transform.forward = movingvec.normalized;

		}

	}
	void FixedUpdate()//Time.fixedDeltaTime
	{
		//rigid.position += movingvec * Time.fixedDeltaTime;
		rigid.position += deltaPos;
		rigid.velocity = new Vector3(movingvec.x,rigid.velocity.y,movingvec.z) + trustvec;
		trustvec = Vector3.zero;
		deltaPos = Vector3.zero;
	}
	private bool checkeState(string stateName,string layerName = "Base Layer")
	{
		int layerIndex = anim.GetLayerIndex(layerName);
		bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
		return result;
	}
	// 
    //  Message block
	// 
	private void OnJumpEnter()
	{
		isNotJump = false;
		trackDirection = true;
		lockPlaner = true;//保留行走的movingvec状态
		pi.InputEnabled = false;//空中无法控制
	}
	private void OnJumpUpdate()
	{
		trustvec = model.transform.up * anim.GetFloat("jumpVec");
	}
	private void OnRollEnter()
	{
		trackDirection = true;
		isNotJump = false;
		trustvec = new Vector3(0.0f,rollVelocity,0.0f);
		lockPlaner = true;//保留行走的movingvec状态
		pi.InputEnabled = false;//空中无法控制
	}
	
	private void OnJabEnter()
	{
		isNotJump = false;
		lockPlaner = true;//保留行走的movingvec状态
		pi.InputEnabled = false;//空中无法控制
	}
	private void OnJabUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("jabVec");
	}
	private void IsGround()
	{
		anim.SetBool("isGround",true);
	}
	private void IsNotGround()
	{
		anim.SetBool("isGround",false);
	}
	private void OnGroundEnter()
	{
		trackDirection = false;
		isNotJump = true;
		lockPlaner = false;
		pi.InputEnabled = true;
		col.material = frictionOne;
	}
	private void OnGroundExit()
	{
		col.material = frictionZero;
	}
	private void OnFailEnter()
	{
		isNotJump = false;
		lockPlaner = true;
		pi.InputEnabled = false;
	}

	//attach layer
	private void OnAttachIdleEnter()
	{
		pi.InputEnabled = true;
		targetLerp = 0.0f;

	}
	private void OnAttachIdleUpdate()
	{
		int layerIndex = anim.GetLayerIndex("Attach");
		float currentWeight = anim.GetLayerWeight(layerIndex);
		currentWeight = Mathf.Lerp(currentWeight,targetLerp,0.1f);
		anim.SetLayerWeight(layerIndex,targetLerp);
	}
	private void OnAttach01HAEnter()
	{
		pi.InputEnabled = false;
		targetLerp = 1.0f;
	}

	private void OnAttach01HAUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attachVec");
		_LerpAttachLayerWeight();
	}
	private void OnAttach01HBEnter()
	{
		pi.InputEnabled = false;
		targetLerp = 1.0f;
	}

	private void OnAttach01HBUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attachVec");
		_LerpAttachLayerWeight();
	}
	private void OnAttach03HAEnter()
	{
		pi.InputEnabled = false;
		targetLerp = 1.0f;
	}

	private void OnAttach03HAUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attachVec");
		_LerpAttachLayerWeight();
	}
	private void _LerpAttachLayerWeight()
	{
		int layerIndex = anim.GetLayerIndex("Attach");
		float currentWeight = anim.GetLayerWeight(layerIndex);
		currentWeight = Mathf.Lerp(currentWeight,targetLerp,0.4f);
		anim.SetLayerWeight(layerIndex,currentWeight);
		if(currentWeight > 0.1f)
			pi.InputEnabled = false;
		else 
			pi.InputEnabled = true;
	}
	private void OnUpdateRM(object _deltaPos)
	{
		if(checkeState("attach01hC","Attach")) //第三段动画变化幅度较大。
			deltaPos += 0.7f*deltaPos + 0.3f*(Vector3)_deltaPos;
	}
}

