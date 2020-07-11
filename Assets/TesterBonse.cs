using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterBonse : MonoBehaviour {

	public SkinnedMeshRenderer srcMeshRenderer;
	public List<SkinnedMeshRenderer> dstMeshRenderers;
	// Use this for initialization
	void Start () {
		foreach (var dstMeshRenderer in dstMeshRenderers) 
		{
			dstMeshRenderer.bones = srcMeshRenderer.bones;
			//print(srcMeshRenderer.bones.Length);
			//print(dstMeshRenderer.bones.Length);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
