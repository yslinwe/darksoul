using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public WeaponManager wm;
	public WeaponData wData;
	// // Use this for initialization
	void Start () {
		wData = GetComponentInChildren<WeaponData>();
	}
	public float GetATK()
	{
		return wData.ATK + wm.am.sm.ATK;
	}
	
	// // Update is called once per frame
	// void Update () {
		
	// }
}
