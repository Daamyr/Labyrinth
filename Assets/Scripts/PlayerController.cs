using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Transform player;

	private Action forward, back, right, left, jump;


	// Use this for initialization
	void Start () {
		forward = new Forward ();
		back = new Back ();
		right = new Right ();
		left = new Left ();
		jump = new Jump ();
	}
	
	// Update is called once per frame
	void Update () {
		handleKey ();


	}

	public void handleKey(){
		//public key binding ?
		if (Input.GetKey (KeyCode.W)) {
			forward.Execute (player, forward);
		}

		if (Input.GetKey (KeyCode.S)) {
			back.Execute (player, back);
		}

		if (Input.GetKey (KeyCode.D)) {
			right.Execute (player, right);
		}

		if (Input.GetKey (KeyCode.A)) {
			left.Execute (player, left);
		}

		if (Input.GetKey (KeyCode.Space)) {
				jump.Execute (player, jump);
		}
	}
}
