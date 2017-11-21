using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createSphere : MonoBehaviour {

	public Button yourButton;
	public GameObject shit;
	public Vector3 position;
	public Quaternion rotation;
	public int couillon;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();

		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		Debug.Log("You have clicked the button!");
		//GameObject clone = Instantiate (shit, position, rotation);
		for (int i = 0; i < couillon; i++) {
			
			Instantiate (shit, position, rotation);
		}
	}	
}
