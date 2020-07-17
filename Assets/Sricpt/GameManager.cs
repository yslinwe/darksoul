using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public WeaponManager testWm;
	private static GameManager instance;
	private DataBase weaponDB;
	private WeaponFactory weaponFact;
	// Use this for initialization
	// Make it a singleton 
	void Awake () {
		CheckGameObject();
		CheckSingle();
	}
	void Start()
	{
		initWeaponDB();
		initWeaponFactory();
		testWm.changeDualHands(true);
		testWm.updateWeaponCollider("R",weaponFact.CreateWeapon("Falchion","R",testWm));
	}
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,100,30),"Falchion"))
		{
			testWm.changeDualHands(true);
			testWm.clearWeapon("L");
			testWm.clearWeapon("R");
			Collider col =weaponFact.CreateWeapon("Falchion","L",testWm);
			testWm.updateWeaponCollider("L",col);
		}
		if(GUI.Button(new Rect(10,50,100,30),"Sword"))
		{
			testWm.changeDualHands(false);
			testWm.clearWeapon("R");
			Collider col =weaponFact.CreateWeapon("Sword","R",testWm);
			testWm.updateWeaponCollider("R",col);
		}
		if(GUI.Button(new Rect(10,90,100,30),"Mace"))
		{
			testWm.changeDualHands(false);
			testWm.clearWeapon("R");
			Collider col =weaponFact.CreateWeapon("Mace","R",testWm);
			testWm.updateWeaponCollider("R",col);
		}
		if(GUI.Button(new Rect(10,130,100,30),"Shield"))
		{
			testWm.changeDualHands(false);
			testWm.clearWeapon("L");
			Collider col =weaponFact.CreateWeapon("Shield","L",testWm);
			testWm.updateWeaponCollider("L",col);
		}
	}
	private void initWeaponDB()
	{
		weaponDB = new DataBase();
	}
	private void initWeaponFactory()
	{
		weaponFact = new WeaponFactory(weaponDB);
	}
	private void CheckGameObject()//检测在规定的gameobject上
	{
		if(tag == "GM")
			return;
		Destroy(this);
	}
	private void CheckSingle() //挂在一个gameobject上
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			return;
		}
		Destroy(this);	
	}
}
