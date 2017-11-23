using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Player player;
	public float forceSaut = 25f;
	public float maxSaut = 10f;

	private Action forward, back, right, left, jump;


	// Use this for initialization
	void Start () {
		forward = new Forward ();
		back = new Back ();
		right = new Right ();
		left = new Left ();
		jump = new Jump (forceSaut, maxSaut, player.GetComponents<Rigidbody>()[0]);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		handleKey ();
	}

	public void OnCollisionEnter(Collision col){
	
		Debug.Log (col.gameObject.name);
		//player.GetComponents<hello> () [0].salut ();
		
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
