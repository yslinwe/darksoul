using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface {
	private CapsuleCollider defCol;
	void Start()
	{
		defCol = GetComponent<CapsuleCollider>();
		defCol.center = Vector3.up;
		defCol.radius = 0.5f;
		defCol.height = 2.0f;
		defCol.isTrigger = true;
	}
	private void OnTriggerEnter(Collider col)
	{

		if(am == null)
			print("null");
		if(col.tag == "Weapon")
		{
			WeaponController targetWc = col.GetComponentInParent<WeaponController>(); //获取攻击方
			GameObject attacker = targetWc.wm.am.gameObject;
			GameObject receiver = am.gameObject;
			Vector3 attackDir = receiver.transform.position - attacker.transform.position; 
			Vector3 counterDir = attacker.transform.position - receiver.transform.position;
			
			//Vector.Angle 不计算方向
			float attackAngle1 = Vector3.Angle(attackDir,attacker.transform.forward); //攻击范围
			float counterAngle1 = Vector3.Angle(receiver.transform.forward,counterDir); //盾反范围
			float counterAngle2 = Vector3.Angle(receiver.transform.forward,attacker.transform.forward); // 判断是否面对面
			bool attackVaild = (attackAngle1 < 45);
			bool counterrVaild = (counterAngle1 < 45) && (Mathf.Abs(counterAngle2 - 180) < 45);
			am.tryDoDamage(targetWc,attackVaild,counterrVaild);
		}
			
	}
}
