using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ActorControl : MonoBehaviour {
	public GameObject model;
	public InputControl pi;
	public float walkSpeed = 1.8f; 
	public float runSpeed = 2.8f;
	public float jumpVelocity = 5.0f;
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
	private bool isNotJump = false;
	private CapsuleCollider col;
	// Use this for initialization
	void Awake () {
		pi    = GetComponent<InputControl>();
		anim  = model.GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		col   = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update ()//Time.deltaTime 1/60
	{	
		float TargetRunMutil = pi.run?2.0f:1.0f;//walk转run
		anim.SetFloat("forward",pi.Dmag*Mathf.Lerp(anim.GetFloat("forward"),TargetRunMutil,0.1f));
		
		if(rigid.velocity.magnitude>5.0f)
			anim.SetTrigger("roll");
		
		if(pi.jump)
		{
			anim.SetTrigger("jump");
			isNotJump = false;
		}
		if(pi.attach&&checkeState("ground")&&isNotJump&&anim.IsInTransition(0)==false)
			anim.SetTrigger("attach");

		if(pi.Dmag>0.1f)//模为0时，pi.Dvec将为(0,0,0)，forword将变回初始值,>0.1f是因为pi里面的Dright和Dup不一定为0
			model.transform.forward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.8f);
		
		if(lockPlaner==false)
			movingvec = model.transform.forward * pi.Dmag * walkSpeed*(pi.run?runSpeed:1.0f);
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
		trustvec = new Vector3(0.0f,jumpVelocity,0.0f);
		lockPlaner = true;//保留行走的movingvec状态
		pi.InputEnabled = false;//空中无法控制
	}

	private void OnRollEnter()
	{
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
		//anim.SetLayerWeight(anim.GetLayerIndex("Attach"),0.0f);
		targetLerp = 0.0f;
	}
	private void OnAttachIdleUpdate()
	{
		_LerpAttachLayerWeight();
	}
	private void OnAttach01HAEnter()
	{
		pi.InputEnabled = false;
		//anim.SetLayerWeight(anim.GetLayerIndex("Attach"),1.0f);
		targetLerp = 1.0f;
	}

	private void OnAttach01HAUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attachVec");
		_LerpAttachLayerWeight();
	}
	private void OnAttach03HAEnter()
	{
		pi.InputEnabled = false;
		//anim.SetLayerWeight(anim.GetLayerIndex("Attach"),1.0f);
	}

	private void OnAttach03HAUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attachVec");
	}
	private void _LerpAttachLayerWeight()
	{
		int layerIndex = anim.GetLayerIndex("Attach");
		float currentWeight = anim.GetLayerWeight(layerIndex);
		currentWeight = Mathf.Lerp(currentWeight,targetLerp,0.4f);
		anim.SetLayerWeight(layerIndex,currentWeight);
	}
	private void OnUpdateRM(object _deltaPos)
	{
		if(checkeState("attach01hC","Attach"))
			deltaPos += (Vector3)_deltaPos;
	}
}

