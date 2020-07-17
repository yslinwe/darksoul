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
			Debug.Log(col.name);
			WeaponController targetWc = col.GetComponentInParent<WeaponController>(); //获取攻击方
			GameObject player = targetWc.wm.am.gameObject;
			GameObject target = am.ac.model;
			// Vector3 attackDir = target.transform.position - player.transform.position; 
			// Vector3 counterDir = player.transform.position - target.transform.position;
			
			// //Vector.Angle 不计算方向
			// float attackAngle1 = Vector3.Angle(attackDir,player.transform.forward); //攻击范围
			// float counterAngle1 = Vector3.Angle(target.transform.forward,counterDir); //盾反范围
			// float counterAngle2 = Vector3.Angle(target.transform.forward,player.transform.forward); // 判断是否面对面
			bool attackVaild = checkAnglePlayer(player,target,70);
			bool counterrVaild = checkAngleTarget(player,target,30);
			am.tryDoDamage(targetWc,attackVaild,counterrVaild);
		}
			
	}
	public bool checkAngleTarget(GameObject player,GameObject target, float targetAngleLimit)
	{
		Vector3 counterDir = player.transform.position - target.transform.position;
		//Vector.Angle 不计算方向
		float counterAngle1 = Vector3.Angle(target.transform.forward,counterDir); //盾反范围
		float counterAngle2 = Vector3.Angle(target.transform.forward,player.transform.forward); // 判断是否面对面
		bool counterrVaild = (counterAngle1 < targetAngleLimit) && (Mathf.Abs(counterAngle2 - 180) < targetAngleLimit);
		return counterrVaild;
	}
	public bool checkAnglePlayer(GameObject player,GameObject target, float playerAngleLimit)
	{
		Vector3 attackDir = target.transform.position - player.transform.position; 
		float attackAngle1 = Vector3.Angle(attackDir,player.transform.forward); //攻击范围
		//Vector.Angle 不计算方向
		bool attackVaild = (attackAngle1 < playerAngleLimit);
		return attackVaild;
	}
}
