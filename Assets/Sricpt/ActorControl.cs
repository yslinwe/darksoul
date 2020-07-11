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
	public bool leftIsShield = true;
	private float targetLerp;
	public Animator anim; //only read
	//[SerializeField]
	private Rigidbody rigid;
	private Vector3 movingvec;
	private Vector3 trustvec;
	private Vector3 deltaPos;
	private bool lockPlaner = false;
	private bool trackDirection = false;//锁定方向判断
	private bool canAttack = false;
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
		//锁定敌人 人物行走动画设置
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
		//翻滚
		if(pi.roll||rigid.velocity.magnitude>7.0f)
		{
			canAttack = false;
			anim.SetTrigger("roll");
		}
		//跳跃
		if(pi.jump)
		{
			anim.SetTrigger("jump");
			canAttack = false;
		}
		//判断左手是否拿盾
		if(leftIsShield)
		{		
			if((checkeState("ground")|| checkeState("blocked"))&&pi.defense)
			{
				anim.SetBool("defense",pi.defense);
				int layerIndex = anim.GetLayerIndex("defense");		
				anim.SetLayerWeight(layerIndex,1);
			}
			else
			{
				anim.SetBool("defense",false);
				int layerIndex = anim.GetLayerIndex("defense");		
				anim.SetLayerWeight(layerIndex,0);
			}
		}
		else 
		{
			int layerIndex = anim.GetLayerIndex("defense");		
			anim.SetLayerWeight(layerIndex,0);
		}
		//左右动画
		if((pi.rb||pi.lb) &&(checkeState("ground")||checkeStateTag("attackR")||checkeStateTag("attackL"))&&canAttack)
		{
			if(pi.rb)
			{
				anim.SetBool("R0L1",false);
				anim.SetTrigger("attack");
			}
			else if(pi.lb&&!leftIsShield)
			{
				anim.SetBool("R0L1",true);
				anim.SetTrigger("attack");
			}
		}
		// do heavy attack
		if((pi.rt||pi.lt) &&(checkeState("ground")||checkeStateTag("attackR")||checkeStateTag("attackL"))&&canAttack)
		{
			if(pi.rt)
			{
				//do right heavy attack
			}
			else
			{
				if(!leftIsShield)
				{
					//do left heavy attack
				}
				else
				{
					anim.SetTrigger("counterBack"); //盾反攻击
				}
			}
		}
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
				if(movingvec != Vector3.zero )
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
	public bool checkeState(string stateName,string layerName = "Base Layer")
	{
		int layerIndex = anim.GetLayerIndex(layerName);
		bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
		return result;
	}
	public bool checkeStateTag(string tagName,string layerName = "Base Layer")
	{
		int layerIndex = anim.GetLayerIndex(layerName);
		bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
		return result;
	}
	// 
    //  Message block
	// 
	private void OnJumpEnter()
	{
		canAttack = false;
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
		canAttack = false;
		trustvec = new Vector3(0.0f,rollVelocity,0.0f);
		lockPlaner = true;//保留行走的movingvec状态
		pi.InputEnabled = false;//空中无法控制
	}
	
	private void OnJabEnter()
	{
		canAttack = false;
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
		canAttack = true;
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
		canAttack = false;
		lockPlaner = true;
		pi.InputEnabled = false;
	}

	//attack layer
	private void OnAttack01HAEnter()
	{
		pi.InputEnabled = false;
	}

	private void OnAttack01HAUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attackVec");
	}
	private void OnAttack01HBEnter()
	{
		pi.InputEnabled = false;
	}

	private void OnAttack01HBUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attackVec");
	}
	private void OnAttack03HAEnter()
	{
		pi.InputEnabled = false;
	}

	private void OnAttack03HAUpdate()
	{
		trustvec = model.transform.forward * anim.GetFloat("attackVec");
	}

	private void OnUpdateRM(object _deltaPos)
	{
		if(checkeState("attack01hC")) //第三段动画变化幅度较大。
			deltaPos += 0.7f*deltaPos + 0.3f*(Vector3)_deltaPos;
	}

	private void OnHitEnter()
	{
		model.SendMessage("WeaponDisable");
		pi.InputEnabled = false;
		lockPlaner = false;
	}
	private void OnDieEnter()
	{
		model.SendMessage("WeaponDisable");
		pi.InputEnabled = false;
		lockPlaner = false;
	}
	private void OnAttackExit()
	{
		model.SendMessage("WeaponDisable");
	}
	private void OnBlockedEnter()
	{
		pi.InputEnabled =false;
	}
	private void OnStunnedEnter()
	{
		pi.InputEnabled =false;
		lockPlaner = false;
	}
	private void OnCounterBackEnter()
	{
		pi.InputEnabled =false;
		lockPlaner = false;
	}
	private void OnCounterBackExit()
	{
		model.SendMessage("CounterBackDisable");
	}
	// 工具方法
	public void issueTrigger(string triggerName)
	{
		anim.SetTrigger(triggerName);
	}
	public void setBool(string boolName, bool value)
	{
		anim.SetBool(boolName,value);
	}
}

