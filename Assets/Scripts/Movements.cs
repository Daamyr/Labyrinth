using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action {

	public float speed = 6.0f;

	public abstract void Execute (Player player, Action action);
	public abstract void Move (Transform player_body);
	//TODO: les actions doivent changer le state
}

public class Forward : Action {
	public override void Execute (Player player, Action action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE FORWARD");
		Vector3 movement = player.forward * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Back : Action {
	public override void Execute (Player player, Action action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE FORWARD");
		Vector3 movement = player.forward * -1 * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Right : Action {
	public override void Execute (Player player, Action action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = player.right * speed * Time.deltaTime;

		player.position += movement;
	}
}
	
public class Left : Action {
	public override void Execute (Player player, Action action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = player.right * -1 * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Jump : Action {
	private float forceSaut;
	private float maxSaut;
	private Rigidbody rig; //pour ajouter la force

	public Jump(float _forceSaut, float _maxSaut, Rigidbody _rig){
		forceSaut = _forceSaut;
		maxSaut = _maxSaut;
		rig = _rig;
	}


	public override void Execute (Player player, Action action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
			rig.AddForce (Vector3.up * forceSaut);
	}
}
