using System.Collections;
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
	public string KeyRB = "k";
	public string KeyLB = "j";
	public string KeyRT = "u";
	public string KeyLT = "i";
	public string KeyJUp = "up";	
	public string KeyJDown = "down";	
	public string KeyJLeft = "left";	
	public string KeyJRight = "right";
	public string KeyJstick = "q";
	public string KeyAction = "h";
	[Header("==== mouse setting ====")]
	public bool mouseEnable = false;
	public float sensitivityX = 1.0f;
	public float sensitivityY = 1.0f;
	public MyButton buttonA = new MyButton(); 
	public MyButton buttonAction = new MyButton(); 
	public MyButton buttonRB = new MyButton(); 
	public MyButton buttonLB = new MyButton(); 
	public MyButton buttonRT = new MyButton(); 
	public MyButton buttonLT = new MyButton(); 
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
		buttonRB.Tick(Input.GetKey(KeyRB));
		buttonLB.Tick(Input.GetKey(KeyLB));
		buttonRT.Tick(Input.GetKey(KeyRT));
		buttonLT.Tick(Input.GetKey(KeyLT));
		buttonJstick.Tick(Input.GetKey(KeyJstick));
		buttonAction.Tick(Input.GetKey(KeyAction));
		run = (buttonA.IsPressing&&!buttonA.IsDelaying)||buttonA.IsExtending;//点击后，延迟开始和结束
		jump = buttonA.OnPressed && buttonA.IsExtending; //双击
		roll = (buttonA.IsDelaying && buttonA.OnReleased); //快速点击
		rb = buttonRB.OnPressed;
		lb = buttonLB.OnPressed;
		rt = buttonRT.OnPressed;
		lt = buttonLT.OnPressed;
		defense = buttonLB.IsPressing;
		lockon = buttonJstick.OnPressed;
		action = buttonAction.OnPressed;
	}
}
        