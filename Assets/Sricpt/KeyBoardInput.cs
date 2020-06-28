﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : IUserInput {
	//Variable
	[Header("====Key setting====")]
	public string KeyUp = "w";
	public string KeyDown = "s";
	public string KeyRight = "d";
	public string KeyLeft = "a";
	public string KeyA = "left shift";
	public string KeyB = "space";
	public string KeyC = "k";
	public string KeyD = "j";
	public string KeyJUp = "up";	
	public string KeyJDown = "down";	
	public string KeyJLeft = "left";	
	public string KeyJRight = "right";
	public string KeyJstick = "q";
	[Header("==== mouse setting ====")]
	public bool mouseEnable = false;
	public float sensitivityX = 1.0f;
	public float sensitivityY = 1.0f;
	public MyButton buttonA = new MyButton(); 
	//public MyButton buttonB = new MyButton(); 
	public MyButton buttonC = new MyButton(); 
	public MyButton buttonD = new MyButton(); 
	public MyButton buttonJstick = new MyButton(); 
	// Update is called once per frame
	void Update () {
		if (mouseEnable)
		{
			Jup = Input.GetAxis("Mouse Y")*3.0f*sensitivityY;
			Jright = Input.GetAxis("Mouse X")*2.5f*sensitivityX;
		}
		else
		{
			Jup = (Input.GetKey(KeyJUp)?1.0f:0) - (Input.GetKey(KeyJDown)?1.0f:0);
			Jright = (Input.GetKey(KeyJRight)?1.0f:0) - (Input.GetKey(KeyJLeft)?1.0f:0);
		}

		TargetDup = InputEnabled?((Input.GetKey(KeyUp)?1.0f:0) - (Input.GetKey(KeyDown)?1.0f:0)):0;
		TargetDright =  InputEnabled?((Input.GetKey(KeyRight)?1.0f:0) - (Input.GetKey(KeyLeft)?1.0f:0)):0;
		Dup = Mathf.SmoothDamp(Dup,TargetDup,ref VectorityDup,0.1f);
		Dright = Mathf.SmoothDamp(Dright,TargetDright,ref VectorityDright,0.1f);
		Vector2 tempAxis = SquareToCircle(new Vector2(Dup,Dright));
		float Dup2 = tempAxis.x;
		float Dright2 = tempAxis.y;
		Dmag = Mathf.Sqrt(Dup2*Dup2+Dright2*Dright2);
		Dvec = Dright2*transform.right+Dup2*transform.forward;

		buttonA.Tick(Input.GetKey(KeyA));
		//buttonB.Tick(Input.GetKey(KeyB));
		buttonC.Tick(Input.GetKey(KeyC));
		buttonD.Tick(Input.GetKey(KeyD));
		buttonJstick.Tick(Input.GetKey(KeyJstick));
		run = (buttonA.IsPressing&&!buttonA.IsDelaying)||buttonA.IsExtending;//点击后，延迟开始和结束
		jump = buttonA.OnPressed && buttonA.IsExtending; //双击
		roll = (buttonA.IsDelaying && buttonA.OnReleased); //快速点击
		attach = buttonC.OnPressed;
		defense = buttonD.IsPressing;
		lockon = buttonJstick.OnPressed;
	}
}
        