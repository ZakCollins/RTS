using UnityEngine;
using System.Collections;

public abstract class RTSObject : MonoBehaviour {

	//All RTS Objects should be selectable with the mouse
	public bool selected;

	//1 is player, 2 is enemy, 0 is neutral
	public int team;

	//The circle or square indicator around the object that means it is selected
	private MeshRenderer selectCircle;

	// Use this for initialization
	void Start () 
	{
		//Get the select circle object
		selectCircle = transform.Find ("Select").GetComponent<MeshRenderer>();
		if (!selectCircle)
		{
			Debug.Log ("Error: no select object");
		}
	


	}


	public void OnSelected()
	{
		selected = true;
		selectCircle.enabled = true;
	}


	public void OnUnselected()
	{
		selected = false;
		selectCircle.enabled = false;
	}


}
