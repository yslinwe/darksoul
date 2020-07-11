using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {
	public ActorControl ac;
	public BattleManager bm;
	public WeaponManager wm;
	public StateManager sm;
	public DriectorManager dm;
	// Use this for initialization
	void Awake () {
		ac = GetComponent<ActorControl>();
		GameObject model = ac.model;
		GameObject sensor = transform.Find("sensor").gameObject;
		bm = sensor.GetComponent<BattleManager>();
		bm = Bind<BattleManager>(sensor);
		wm = Bind<WeaponManager>(model);
		sm = Bind<StateManager>(gameObject);
		dm = Bind<DriectorManager>(gameObject);
		
	}
	private T Bind<T>(GameObject go) where T : IActorManagerInterface
	{
		T tempInstance = go.GetComponent<T>();
		if(tempInstance == null)
		{
			tempInstance = go.AddComponent<T>();
		}
		tempInstance.am = this;
		return tempInstance;
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void tryDoDamage(WeaponController targetWc, bool attackVaild, bool counterVaild)
	{
		if(sm.isCounterBack)
		{
			if(sm.isCounterBackSuccess)
			{
				if(counterVaild)
				{
					targetWc.wm.am.Stunned();
					//CounterBack();
				}
			}
			else if(sm.isCounterBackFailure)
			{
				if(attackVaild) 
					HitOrDie(false);
			}
		}
		else if(sm.isImmortal)
		{
			//do nothing
		}
		else if(sm.isDefense)
		{
			//Attack should be blocked
			if(attackVaild) 
				Blocked();
		}
		else
		{
			if(attackVaild) 
				HitOrDie(true);
		}
	}
	public void CounterBack()
	{
		ac.issueTrigger("counterBack");
	}
	public void Stunned()
	{
		ac.issueTrigger("stunned");
	}
	public void Blocked()
	{
		ac.issueTrigger("blocked");
	}
	public void Hit()
	{
		ac.issueTrigger("hit");
	}
	public void Die()
	{
		ac.issueTrigger("die");
		ac.pi.InputEnabled = false; //耦合
		if(ac.camCon.lockState == true)
		{
			ac.camCon.LockUnlock();
		}
			ac.camCon.enabled = false;

	}
	public void HitOrDie(bool isAnimationHit)
	{
		if(sm.HP<=0)
		{
			//Already dead
		}
		else
		{
			sm.changeHp(-5);
			if(sm.HP > 0)
			{
				if(isAnimationHit)
					Hit();
				else
				{
					//做粒子特效或者其他的
				}
			}else
			{
				Die();
			}
		}
	} 
	public void setCounterBack(bool value)
	{
		sm.isCounterBackEnabled = value;
	}
	public void lockActorController()
	{
		ac.setBool("lock",true);
	}
	public void UnlockActorController()
	{
		ac.setBool("lock",false);
	}
}
