using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall /*: MonoBehaviour*/ {

	//cell
	enum FACING { NORTH, EAST, SOUTH, WEST };

	//WallTexture texture;
	Texture2D texture;

	public Wall(){
		GraphicManager graphicManager = GraphicManager.getInstance ();
		texture = graphicManager.LoadTexture ("wall");
		Debug.Log ("\tWall created with texture " + texture);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
