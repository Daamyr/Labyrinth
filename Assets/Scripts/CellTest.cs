using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTest {

    GameObject floorPrefab;
    GameObject wallPrefab;

    public GameObject FloorPrefab { get; set; }
    public GameObject WallPrefab { get; set; }

    GameObject floor;
    GameObject wallN;
    GameObject wallE;
    GameObject wallS;
    GameObject wallW;

    Vector3 cordinates;

    public CellTest(Vector3 _cordinates)
    {
        cordinates = _cordinates;
        Debug.Log("CellTest-> " + cordinates.ToString());
        //floor = Instantiate(floorPrefab, cordinates, floorPrefab.transform.rotation) as GameObject;
        //Instantiate(floorPrefab);
    }

    public void Draw()
    {
        Debug.Log("draw-> " + floorPrefab);
        //floor = Instantiate(floorPrefab, cordinates, floorPrefab.transform.rotation) as GameObject;
    }
    /*
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */
}
