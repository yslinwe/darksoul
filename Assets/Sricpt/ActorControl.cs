using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ActorControl : MonoBehaviour {
	public GameObject model;
	public InputControl pi;
	public float walkSpeed = 1.8f; 
	public float runSpeed = 2.8f;
	public float jumpVelocity = 3.0f;
	[SerializeField]
	private Animator anim;
	private Rigidbody rigid;
	private Vector3 movingvec;
	private Vector3 trustvec;
	private bool lockPlaner = false;
	// Use this for initialization
	void Awake () {
		pi = GetComponent<InputControl>();
		anim = model.GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()//Time.deltaTime 1/60
	{	
		float TargetRunMutil = pi.run?2.0f:1.0f;//walk转run
		anim.SetFloat("forward",pi.Dmag*Mathf.Lerp(anim.GetFloat("forward"),TargetRunMutil,0.1f));
		if(pi.jump)
		anim.SetTrigger("jump");

		if(pi.Dmag>0.1f)//模为0时，pi.Dvec将为(0,0,0)，forword将变回初始值,>0.1f是因为pi里面的Dright和Dup不一定为0
			model.transform.forward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.8f);
		
		if(lockPlaner==false)
			movingvec = model.transform.forward * pi.Dmag * walkSpeed*(pi.run?runSpeed:1.0f);
	}
	void FixedUpdate()//Time.fixedDeltaTime
	{
		//rigid.position += movingvec * Time.fixedDeltaTime;
		rigid.velocity = new Vector3(movingvec.x,rigid.velocity.y,movingvec.z) + trustvec;
		trustvec = Vector3.zero;
	}
	// 
    //  Message block
	// 
	private void OnJumpEnter()
	{
		trustvec = new Vector3(0.0f,jumpVelocity,0.0f);
		lockPlaner = true;//保留行走的movingvec状态
		pi.InputEnabled = false;//空中无法控制
	}

	private void IsGround()
	{
		lockPlaner = false;
		anim.SetBool("isGround",true);
	}
	private void IsNotGround()
	{
		anim.SetBool("isGround",false);
	}
	private void OnGroundEnter()
	{
		print("")
		lockPlaner = false;
		pi.InputEnabled = true;
	}
	private void OnFailEnter()
	{
		lockPlaner = true;
		pi.InputEnabled = false;
	}
}

