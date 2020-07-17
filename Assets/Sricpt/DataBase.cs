using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase {
	private string weaponDataFileName = "weaponData";
	public readonly JSONObject weaponJsonData;
	public DataBase()
	{
		TextAsset weaponContent = Resources.Load(weaponDataFileName) as TextAsset;
		weaponJsonData = new JSONObject(weaponContent.text);
	}
}
