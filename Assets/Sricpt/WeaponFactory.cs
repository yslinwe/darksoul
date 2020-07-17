using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory {
	private DataBase weaponData;
	public WeaponFactory(DataBase _weaponData)
	{
		weaponData = _weaponData;
	}
	public GameObject CreateWeapon(string weaponName,Vector3 pos , Quaternion rot)
	{
		GameObject weapon = Resources.Load(weaponName) as GameObject;
		GameObject weaponObj = GameObject.Instantiate(weapon,pos,rot);
		WeaponData wData = weaponObj.AddComponent<WeaponData>();
		wData.ATK = weaponData.weaponJsonData[weaponName]["ATK"].f;
		return weaponObj;
	}
	public Collider CreateWeapon(string weaponName, string side, WeaponManager wm)
	{
		WeaponController wc;
		if(side == "R")
		{
			wc = wm.wcR;
		}
		else if(side == "L")
		{
			wc = wm.wcL;
			if(weaponName == "Shield")
				wm.am.ac.leftIsShield = true;
			else
				wm.am.ac.leftIsShield = false;
		}
		else
			return null;

		GameObject weapon = Resources.Load(weaponName) as GameObject;
		GameObject weaponObj = GameObject.Instantiate(weapon);
	
		WeaponData wData = weaponObj.AddComponent<WeaponData>();
		wData.ATK = weaponData.weaponJsonData[weaponName]["ATK"].f;
		wc.wData = wData;

		weaponObj.transform.parent = wc.transform;
		weaponObj.transform.localPosition = Vector3.zero;
		weaponObj.transform.localRotation = Quaternion.identity;
		Collider col = weaponObj.GetComponentInChildren<Collider>();
		col.enabled = false;
		col.isTrigger = true;
		return col;
	}
}
