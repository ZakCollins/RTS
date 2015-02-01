using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Code modified from https://paulbutera.wordpress.com/2013/04/04/unity-rts-tutorial-part-1-marquee-selection-of-units/

//Known Bugs: Should not let you select multiple enemies. Maybe only allow you to select your own units by dragging and enemies can only be selected by a click.
//Should probably tell the PlayerController what units are selected so the player controller can give them orders.
//You could also just check to see if any player units have been selected and if they have then don't select enemies. If an enemy has been selected and you start to select you unit than deselect the enemy

public class CameraMarquee : MonoBehaviour 
{
	public Texture marqueeGraphics;
	private Vector2 marqueeOrigin;
	private Vector2 marqueeSize;
	public Rect marqueeRect;
	public List<GameObject> SelectableUnits; //get from GameController
	private Rect backupRect;


	private void OnGUI()
	{
		marqueeRect = new Rect(marqueeOrigin.x, marqueeOrigin.y, marqueeSize.x, marqueeSize.y);
		GUI.color = new Color(0, 0, 50, .1f);
		GUI.DrawTexture(marqueeRect, marqueeGraphics);
	}


	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			//Populate the selectableUnits array with all the selectable units that exist
			//SelectableUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("SelectableUnit")); // get from game controller
			SelectableUnits = new List<GameObject>(GameController.gameController.allRTSObjects);
			
			float _invertedY = Screen.height - Input.mousePosition.y;
			marqueeOrigin = new Vector2(Input.mousePosition.x, _invertedY);

			//Probably won't use
			//Check if the player just wants to select a single unit opposed to drawing a marquee and selecting a range of units
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			//Maybe use this with a layer mask if hitting the ground is a problem
			if (Physics.Raycast(ray, out hit))
			{
				SelectableUnits.Remove(hit.transform.gameObject);
				hit.transform.gameObject.SendMessage("OnSelected",SendMessageOptions.DontRequireReceiver);
			}


		}

		if (Input.GetMouseButtonUp(0))
		{
			//Reset the marquee so it no longer appears on the screen.
			marqueeRect.width = 0;
			marqueeRect.height = 0;
			backupRect.width = 0;
			backupRect.height = 0;
			marqueeSize = Vector2.zero;
			
		}


		if (Input.GetMouseButton(0))
		{
			float _invertedY = Screen.height - Input.mousePosition.y;
			marqueeSize = new Vector2(Input.mousePosition.x - marqueeOrigin.x, (marqueeOrigin.y - _invertedY) * -1);
	
			//FIX FOR RECT.CONTAINS NOT ACCEPTING NEGATIVE VALUES
			if (marqueeRect.width < 0)
			{
				backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width), marqueeRect.y, Mathf.Abs(marqueeRect.width), marqueeRect.height);
			}
			else if (marqueeRect.height < 0)
			{
				backupRect = new Rect(marqueeRect.x, marqueeRect.y - Mathf.Abs(marqueeRect.height), marqueeRect.width, Mathf.Abs(marqueeRect.height));
			}
			if (marqueeRect.width < 0 && marqueeRect.height < 0)
			{
				backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width), marqueeRect.y - Mathf.Abs(marqueeRect.height), Mathf.Abs(marqueeRect.width), Mathf.Abs(marqueeRect.height));
			}


			foreach (GameObject unit in SelectableUnits)
			{
				//Convert the world position of the unit to a screen position and then to a GUI point
				Vector3 _screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
				Vector2 _screenPoint = new Vector2(_screenPos.x, Screen.height - _screenPos.y);
				//Ensure that any units not within the marquee are currently unselected
				if (!marqueeRect.Contains(_screenPoint) || !backupRect.Contains(_screenPoint))
				{
					unit.SendMessage("OnUnselected", SendMessageOptions.DontRequireReceiver);
				}
				if (marqueeRect.Contains(_screenPoint) || backupRect.Contains(_screenPoint))
				{
					unit.SendMessage("OnSelected", SendMessageOptions.DontRequireReceiver);
				}
			}
		}


	}





}
