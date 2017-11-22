using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action {

	public float speed = 5.0f;

	public abstract void Execute (Transform player, Action action);
	public virtual void Move (Transform player){
		Debug.Log ("Move from the Action class");
		Move (player);
	}
	//TODO: les actions doivent changer le state
}

public class Forward : Action {
	public override void Execute (Transform player, Action action){
		Move (player);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE FORWARD");
		Vector3 movement = Vector3.forward * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Back : Action {
	public override void Execute (Transform player, Action action){
		Move (player);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE FORWARD");
		Vector3 movement = Vector3.back * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Right : Action {
	public override void Execute (Transform player, Action action){
		Move (player);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = Vector3.right * speed * Time.deltaTime;

		player.position += movement;
	}
}
	
public class Left : Action {
	public override void Execute (Transform player, Action action){
		Move (player);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = Vector3.left * speed * Time.deltaTime;

		player.position += movement;
	}
}

public class Jump : Action {
	public override void Execute (Transform player, Action action){
		Move (player);
	}

	public override void Move (Transform player){
		//		Debug.Log ("MOVE RIGHT");
		Vector3 movement = Vector3.up * speed * Time.deltaTime;

		player.position += movement;
	}
}
