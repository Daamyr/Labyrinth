using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    CellTest test;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			this.GetComponents<Rigidbody> () [0].isKinematic = false;
            test = new CellTest(new Vector3(0,20,0));
            test.FloorPrefab = floorPrefab;
            test.Draw();

        }
	}
}
