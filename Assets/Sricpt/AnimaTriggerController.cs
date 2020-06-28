using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaTriggerController : MonoBehaviour {

	private Animator anim;
	void Awake()
	{
		anim = GetComponent<Animator>();
	}
	private void ResetTrigge(string triggerName) //让攻击不是那么容连招
	{
		anim.ResetTrigger(triggerName);
	}
}
