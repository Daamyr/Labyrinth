using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AAction {

	public float speed = 6.0f;

	public abstract void Execute (Player player, AAction action);
	public abstract void Move (Transform player_body);
}

public class Forward : AAction {
	public override void Execute (Player player, AAction action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE FORWARD");
		Vector3 movement = player.forward * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Back : AAction {
	public override void Execute (Player player, AAction action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE BACK");
		Vector3 movement = player.forward * -1 * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Right : AAction {
	public override void Execute (Player player, AAction action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = player.right * speed * Time.deltaTime;

		player.position += movement;
	}
}
	
public class Left : AAction {
	public override void Execute (Player player, AAction action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = player.right * -1 * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Plane : AAction {
	private float forceSaut;
	private Rigidbody rig; //pour ajouter la force

	public Plane(float _forceSaut, Rigidbody _rig){
		forceSaut = _forceSaut;
		rig = _rig;
	}


	public override void Execute (Player player, AAction action){
		Move (player.gameObject.transform);
	}

	public override void Move (Transform player){
			rig.AddForce (Vector3.up * forceSaut);
	}
}
