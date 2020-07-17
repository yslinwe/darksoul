using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActionManager :IActorManagerInterface {

	public List<CasterEventManager> overlapEcastms = new List<CasterEventManager>();
	//public CapsuleCollider col;
	// Use this for initialization
	// void Start () {
		
	// }
	
	// // Update is called once per frame
	// void Update () {
		
	// }
	void OnTriggerEnter(Collider col)
	{
		CasterEventManager[] ecastms = col.GetComponents<CasterEventManager>();
		foreach (var ecastm in ecastms)
		{
			if(!overlapEcastms.Contains(ecastm))
			{
				overlapEcastms.Add(ecastm);
			}
		}
	}
	void OnTriggerExit(Collider col)
	{
		CasterEventManager[] ecastms = col.GetComponents<CasterEventManager>();
		foreach (var ecastm in ecastms)
		{
			if(overlapEcastms.Contains(ecastm))
			{
				overlapEcastms.Remove(ecastm);
			}
		}
	}
}
