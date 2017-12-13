using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createSphere : MonoBehaviour {

	public GameObject shit;
	//public Vector3 position;
	public Quaternion rotation;
	//public int couillon;

	void Start()
	{
		//Button btn = yourButton.GetComponent<Button>();

		//btn.onClick.AddListener(TaskOnClick);

	}

	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)){
			Debug.Log (this.transform.position);
			Instantiate (this, this.transform.position, this.rotation);

			//Wall wall = new Wall ();
		}

	}

	void TaskOnClick()
	{
		Debug.Log("You have clicked the button!");
		if (shit == null) {
			Debug.Log ("You have to set an object in order to duplicate it");
		} else {
			//GameObject clone = Instantiate (shit, position, rotation);
//			for (int i = 0; i < couillon; i++) {
//				Instantiate (shit, position, rotation);
//			}
		}
	}	
}
