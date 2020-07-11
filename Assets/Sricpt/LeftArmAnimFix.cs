using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour {
	private Animator anim;
	private ActorControl ac;
	public Vector3 a; 
	void Awake()
	{
		anim = GetComponent<Animator>();
		ac = GetComponentInParent<ActorControl>();//耗能
	}
	void OnAnimatorIK()
	{
		if(ac.leftIsShield)
		{
			if(anim.GetBool("defense")==false)
			{
				Transform leftLowArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
				leftLowArm.localEulerAngles += 0.75f*a;
				anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm,Quaternion.Euler(leftLowArm.localEulerAngles));
			}
		}
	}
}
