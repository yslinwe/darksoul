using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour { //设置成抽象类，无法new 不会出现的面板上

	[Header("====Output signals====")]
	public float Jup;
	public float Jright;
	public float Dup;
	public float Dright;
	public float Dmag;
	public Vector3 Dvec;
	//1.pressing signal
	public bool run;
	public bool defense;
	//2.trigger signal
	public bool attach;
	public bool roll;
	public bool lockon;
	//3.double signal
	public bool jump;
	[Header("====others====")]
	public bool InputEnabled = true;
	protected float TargetDup;
	protected float TargetDright;	
	protected float VectorityDup;
	protected float VectorityDright;

	protected Vector2 SquareToCircle(Vector2 Input)
	{
		Vector2 Output = Vector2.zero;
		Output.x = Input.x*Mathf.Sqrt(1-Input.y*Input.y/2.0f);
		Output.y = Input.y*Mathf.Sqrt(1-Input.x*Input.x/2.0f);
		return Output;
	}
}
