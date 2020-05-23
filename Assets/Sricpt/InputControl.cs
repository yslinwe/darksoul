using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour {
	//Variable
	[Header("====Key setting====")]
	public string KeyUp = "w";
	public string KeyDown = "s";
	public string KeyRight = "d";
	public string KeyLeft = "a";
	public string KeyA = "left shift";
	public string KeyB = "space";
	public string KeyC;
	public string KeyD;	
	[Header("====Output signals====")]
	public float Dup;
	public float Dright;
	public float Dmag;
	public Vector3 Dvec;
	//1.pressing signal
	public bool run;
	//2.trigger signal
	public bool jump;
	public bool lastjump;
	//3.double signal
	[Header("====others====")]
	public bool InputEnabled = true;
	private float TargetDup;
	private float TargetDright;	
	private float VectorityDup;
	private float VectorityDright;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TargetDup = InputEnabled?((Input.GetKey(KeyUp)?1.0f:0) - (Input.GetKey(KeyDown)?1.0f:0)):0;
		TargetDright =  InputEnabled?((Input.GetKey(KeyRight)?1.0f:0) - (Input.GetKey(KeyLeft)?1.0f:0)):0;
		Dup = Mathf.SmoothDamp(Dup,TargetDup,ref VectorityDup,0.1f);
		Dright = Mathf.SmoothDamp(Dright,TargetDright,ref VectorityDright,0.1f);
		Vector2 tempAxis = SquareToCircle(new Vector2(Dup,Dright));
		float Dup2 = tempAxis.x;
		float Dright2 = tempAxis.y;
		Dmag = Mathf.Sqrt(Dup2*Dup2+Dright2*Dright2);
		Dvec = Dright2*transform.right+Dup2*transform.forward;
		run = Input.GetKey(KeyA);
		bool newjump = Input.GetKey(KeyB);
		if(newjump!=lastjump&&newjump==true)
			jump = true;
		else
			jump = false;
		lastjump = newjump;
		//jump = Input.GetKeyDown(KeyB);
	}

	private Vector2 SquareToCircle(Vector2 Input)
	{
		Vector2 Output = Vector2.zero;
		Output.x = Input.x*Mathf.Sqrt(1-Input.y*Input.y/2.0f);
		Output.y = Input.y*Mathf.Sqrt(1-Input.x*Input.x/2.0f);
		return Output;
	}
}
