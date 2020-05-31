using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaTriggerController : MonoBehaviour {

	private Animator anim;
	void Awake()
	{
		anim = GetComponent<Animator>();
	}
	private void ResetTrigge(string triggerName)
	{
		print(triggerName);
		anim.ResetTrigger(triggerName);
	}
}
