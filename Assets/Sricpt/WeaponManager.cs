using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface {

	public GameObject whL;
	public GameObject whR;
	public WeaponController wcL;
	public WeaponController wcR;
	[SerializeField]
	private Collider weaponColL;
	[SerializeField]
	private Collider weaponColR;
	void Start()
	{
		try
		{
			whR = transform.DeepFind("weaponHandleR").gameObject;
			wcR = BindWeaponController(whR);
			weaponColR = whR.GetComponentInChildren<Collider>();
		}
		catch (System.Exception)
		{
			//Debug.Log("there is not weaponHandleR"); 
			//throw e;
		}

		try
		{
			whL = transform.DeepFind("weaponHandleL").gameObject;
			wcL = BindWeaponController(whL);
			weaponColL = whL.GetComponentInChildren<Collider>();
		}
		catch (System.Exception)
		{
			
			//Debug.Log("there is not weaponHandleL"); 
		}
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
		if (am.ac.checkeStateTag("attackL")&&weaponColL !=null)
		{
			weaponColL.enabled = true;
		}
		if(weaponColR != null)
		{
			weaponColR.enabled = true;
		}
	} 
	public void WeaponDisable()
	{
		if(weaponColL !=null )
		{
			weaponColL.enabled = false;
		}
		if(weaponColR != null)
		{
			weaponColR.enabled = false;
		}
	}
	public void CounterBackEnable()
	{
		am.setCounterBack(true);
	}
	public void CounterBackDisable()
	{
		am.setCounterBack(false);
	}
	public void updateWeaponCollider(string side, Collider col)
	{
		if(side == "L")
			weaponColL = col;
		else if(side == "R")
			weaponColR =col;
	}
	public void clearWeapon(string side)
	{
		if(side == "L")
		{
			weaponColL = null;
			wcL.wData = null;
			foreach (Transform tran in whL.transform)
			{
				Destroy(tran.gameObject);
			}
		}
		else if(side == "R")
		{
			weaponColR = null;
			wcR.wData = null;
			foreach (Transform tran in whR.transform)
			{
				Destroy(tran.gameObject);
			}
		}
	}
	public void changeDualHands(bool dualON)
	{
		am.changeDualHands(dualON);
	}
}
