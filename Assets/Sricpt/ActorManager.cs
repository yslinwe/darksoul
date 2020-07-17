using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {
	public ActorControl ac;
	public BattleManager bm;
	public WeaponManager wm;
	public StateManager sm;
	public DriectorManager dm;
	public InterActionManager im;
	public AnimatorOverrideController oneHand;
	public AnimatorOverrideController twoHand;
	// Use this for initialization
	void Awake () {
		ac = GetComponent<ActorControl>();
		GameObject model = ac.model;
		GameObject sensor = null;
		try
		{
			 sensor = transform.Find("sensor").gameObject;			
		}
		catch (System.Exception)
		{
			
			//throw;
			// there is not sensor or relate object
			//
		}

		bm = Bind<BattleManager>(sensor);
		wm = Bind<WeaponManager>(model);
		sm = Bind<StateManager>(gameObject);
		dm = Bind<DriectorManager>(gameObject);
		im = Bind<InterActionManager>(sensor);
		
		ac.OnAction += doAction;
	}
	private T Bind<T>(GameObject go) where T : IActorManagerInterface
	{
		if(go == null)
			return null;
		T tempInstance = go.GetComponent<T>();
		if(tempInstance == null)
		{
			tempInstance = go.AddComponent<T>();
		}
		tempInstance.am = this;
		return tempInstance;
	}
	// // Update is called once per frame
	// void Update () {
		
	// }
	public void doAction()
	{
		foreach (var ecastm in im.overlapEcastms)
		{
			if(ecastm.eventName == "frontStab" && ecastm.active &&!dm.isPlayTimeline())
			{
				if(bm.checkAngleTarget(ac.model,ecastm.am.gameObject,15))
				{
					transform.position = ecastm.am.transform.position + ecastm.am.transform.TransformVector(ecastm.offest);
					ac.model.transform.LookAt(ecastm.am.transform,Vector3.up);
					dm.playFrontStab("frontStab",this,ecastm.am);
				}
			}
			else if(ecastm.eventName == "openBox" && ecastm.active &&!dm.isPlayTimeline())
			{
				if(bm.checkAngleTarget(ac.model,ecastm.am.gameObject,15))
				{
					transform.position = ecastm.am.transform.position + ecastm.am.transform.TransformVector(ecastm.offest);
					ac.model.transform.LookAt(ecastm.am.transform,Vector3.up);
					ecastm.active = false;
					dm.playFrontStab("openBox",this,ecastm.am);
				}
			} 
			else if(ecastm.eventName == "LeverUp" && ecastm.active &&!dm.isPlayTimeline())
			{
				if(bm.checkAngleTarget(ac.model,ecastm.am.gameObject,15))
				{
					transform.position = ecastm.am.transform.position + ecastm.am.transform.TransformVector(ecastm.offest);
					ac.model.transform.LookAt(ecastm.am.transform,Vector3.up);
					//ecastm.active = false;
					dm.playFrontStab("LeverUp",this,ecastm.am);
				}
			} 
		}
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
					HitOrDie(targetWc,false);
			}
		}
		else if(sm.isImmortal)
		{
			//do nothing
		}
		else if(sm.isDefense&&counterVaild)
		{
			//Attack should be blocked
			Blocked();
		}
		else
		{
			if(attackVaild) 
				HitOrDie(targetWc,true);
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
	public void HitOrDie(WeaponController targetWc, bool isAnimationHit)
	{
		if(sm.HP<=0)
		{
			//Already dead
		}
		else
		{
			sm.changeHp(-1*targetWc.GetATK());
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
	public void changeDualHands(bool dualON)
	{
		if(dualON)
		{
			ac.anim.runtimeAnimatorController = twoHand;
		}
		else
		{
			ac.anim.runtimeAnimatorController = oneHand;
		}
	}
}
