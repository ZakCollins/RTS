using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//Singleton design pattern
	public static GameController gameController;

	public List<GameObject> allRTSObjects;


	void Awake () {
		//Makes it so there can only be 1 of this at any given time
		if (gameController == null)
		{
			DontDestroyOnLoad(gameObject);
			gameController = this;
		}
		else if (gameController != this)
		{
			Destroy(gameObject);
		}

		
	}


	void Start () 
	{
		allRTSObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("SelectableObject"));

	
	}
	
	//Keep track of all RTS Objects
	//RTS Objects will call this when created
	public void AddRTSObject(GameObject rtsobject)
	{
		allRTSObjects.Add (rtsobject);
	}

	//RTS Objects will call this when destroyed
	public void RemoveRTSObject(GameObject rtsobject)
	{
		allRTSObjects.Remove (rtsobject);
	}



}
