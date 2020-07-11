using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyIUserInput : IUserInput {

	// Use this for initialization
	IEnumerator Start () {
		rb = true;
		yield return 0;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDmagDvec(Dup,Dright);
	}
}
