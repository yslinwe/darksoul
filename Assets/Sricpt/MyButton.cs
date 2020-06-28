using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton{

	// 1. 提供IsPressing 信号，让后续模块得知目前用户硬件输入状态
	// 2.OnPressed 信号，提供目前是否刚刚按下此按钮
	// 3.OnReleased信号，提供目前是否刚刚释放此按钮
	// IsExtending 刚刚释放此按钮后，是否在延申时间内
	// IsDelaying 再按下按钮之前，延迟的时间内
	public bool IsPressing = false;
	public bool	OnPressed  = false;
	public bool OnReleased = false;
	public float extendingDuration = 0.3f;
	public float delayingDuration = 0.3f;
	public bool IsExtending = false;
	public bool IsDelaying = false;
	private bool curState = false;
	private bool lastState = false;
	private MyTimer extendTimer = new MyTimer();
	private MyTimer delayTimer = new MyTimer();
	public void Tick(bool input)
	{
		extendTimer.Tick();
		delayTimer.Tick();
		curState = input;
		IsPressing = curState;
		OnPressed = false;
		OnReleased = false;
		if(curState != lastState)
		{
			if(curState == true)
			{
				OnPressed = true;
				StartTimer(delayTimer,extendingDuration);
			}
			else
			{
				OnReleased = true;
				StartTimer(extendTimer,extendingDuration);
			}
		}
		lastState = curState;
		IsExtending = extendTimer.state == MyTimer.STATE.RUN;
		IsDelaying = delayTimer.state == MyTimer.STATE.RUN;
	}
	public void StartTimer(MyTimer timer , float duration)
	{
		timer.duration = duration;
		timer.GO();
	}
}
