using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface {

	public GameObject whL;
	public GameObject whR;
	public WeaponController wcL;
	public WeaponController wcR;

	private Collider weaponColL;
	private Collider weaponColR;
	void Start()
	{
		whR = transform.DeepFind("weaponHandleR").gameObject;
		whL = transform.DeepFind("weaponHandleL").gameObject;

		if(whR==null)
			Debug.LogError("can not find whR");
		whL = transform.DeepFind("weaponHandleL").gameObject;
		if(whL==null)
			Debug.LogError("can not find whL");

		wcR = BindWeaponController(whR);
		wcL = BindWeaponController(whL);

		weaponColR = whR.GetComponentInChildren<Collider>();
		weaponColL = whL.GetComponentInChildren<Collider>();
		WeaponDisable();
	}
	private WeaponController BindWeaponController(GameObject targetObj)
	{
		WeaponController tempWc;
		tempWc = targetObj.GetComponent<WeaponController>();
		if(tempWc == null)
		{
			tempWc = targetObj.AddComponent<WeaponController>();
		}
		tempWc.wm = this;
		return tempWc;
	}
	public void WeaponEnable()
	{
		if (am.ac.checkeStateTag("attackL"))
		{
			weaponColL.enabled = true;
		}
		else
		{
			weaponColR.enabled = true;
		}
	} 
	public void WeaponDisable()
	{
		weaponColL.enabled = false;
		weaponColR.enabled = false;
	}
	public void CounterBackEnable()
	{
		am.setCounterBack(true);
	}
	public void CounterBackDisable()
	{
		am.setCounterBack(false);
	}
}
