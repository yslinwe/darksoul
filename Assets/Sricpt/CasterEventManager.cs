using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEventManager : IActorManagerInterface {

	public string eventName;
	public bool active;
	public Vector3 offest = new Vector3(0,0,1.0f); 
	
	// // Use this for initialization
	void Start () {
		if(am == null)
			am = GetComponentInParent<ActorManager>();
	}
	
	// // Update is called once per frame
	// void Update () {
		
	// }
	// void OnTriggerStay(Collider col)
	// {
	// 	Debug.Log(col.name);
	// }
}
