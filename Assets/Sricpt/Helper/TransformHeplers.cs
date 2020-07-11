using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHeplers{
	public static Transform DeepFind(this Transform parent, string targetName)
	{
		Transform tempTransform = null;
		foreach(Transform child in parent)
		{
			if(child.name == targetName)
				return child;
			else
			{
				tempTransform = DeepFind(child,targetName);
				if(tempTransform != null)
					return tempTransform;
			}  
		}
		return null;
	}
}
