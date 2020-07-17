using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface {
	
	public float maxHP = 100.0f;
	public float HP = 15.0f;
	public float ATK = 1.0f;
	[Header("1st order state flags")]
	public bool isGround;
	public bool isJump;
	public bool isFall;
	public bool isRoll;
	public bool isJab;
	public bool isAttack;
	public bool isHit;
	public bool isDie;
	public bool isBlocked;
	public bool isDefense;
	public bool isCounterBack;
	public bool isCounterBackEnabled;
	[Header("2nd order state flags")]
	public bool isAllowDenfense;
	public bool isImmortal;
	public bool isCounterBackSuccess;
	public bool isCounterBackFailure;
	void Start()
	{
		HP = maxHP;
	}
	void Update()
	{
		isGround = am.ac.checkeState("ground");
		isJump = am.ac.checkeState("jump");
		isFall = am.ac.checkeState("fall");
		isRoll = am.ac.checkeState("roll");
		isJab = am.ac.checkeState("jab");
		isAttack = am.ac.checkeStateTag("attackR")||am.ac.checkeStateTag("attackL");
		isHit = am.ac.checkeState("hit");
		isDie = am.ac.checkeState("die");
		isBlocked = am.ac.checkeState("blocked");
		isCounterBack = am.ac.checkeState("counterBack");
		//isCounterBackEnabled = true;
		isAllowDenfense = isBlocked || isGround;
		isDefense = isAllowDenfense && am.ac.checkeState("defense1h","defense"); 
		isImmortal = isRoll || isJab; //无敌状态
		isCounterBackSuccess = isCounterBackEnabled;
		isCounterBackFailure = isCounterBack && !isCounterBackEnabled;
	}
	public void changeHp(float value)
	{
		HP += value;
		Mathf.Clamp(HP,0,maxHP);
	}
}
