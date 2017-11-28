using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    private Vector2Int coordinates;
    private GameObject floorPrefab;
    private GameObject wallPrefab;

    public Vector2Int Coordinates { get; set; }
    public GameObject FloorPrefab { get; set; }
    public GameObject WallPrefab { get; set; }


    bool visited = false;

    public Cell(Vector2Int _coordinates)
    {
        coordinates = _coordinates;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(coordinates.x <= 20)
        {
            Debug.Log("YO" + coordinates.ToString() + GetComponentInParent<Transform>().position.x);
            //GetComponentInParent<Transform>()[0].position.x;
        }
	}
}
