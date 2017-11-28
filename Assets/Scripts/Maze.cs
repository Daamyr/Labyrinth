﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vector2Int size;

    public int manualFloorScaleX; //TODO: acceder au prefab du Floor pour remplacer ces lignes
    public int manualFloorScaleY;
    public int manualFloorScaleZ;

    public GameObject floorPrefab;

    public Cell cellObject;

    private Cell[,] cells;

    // Use this for initialization
    void Start()
    {
        cells = new Cell[size.x, size.y];
    }

    // Update is called once per frame
    void Update()
    {

        /* UTILISER UNE COROUTINE
    private int renduX;
    private int renduZ;
        if(renduX < sizeX)
        {
            if(renduZ < sizeZ)
            {
                CreateCell(renduX, renduZ);
                renduZ++;
            }
            else
            {
                renduX++;
                renduZ = 0;
            }
        }
        */

    }

    public float generationStepDelay;

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new Cell[size.x, size.y];
        for (int i = 0; i < size.x; i++)
        {
            CreateCell(new Vector2Int(i, 0));
            yield return delay;
        }
    }

    //TODO: Move this code to the a factory
    private void CreateCell(Vector2Int coordinates)
    {
        Cell newCell = Instantiate(cellObject) as Cell;
        newCell.Coordinates = coordinates;
        newCell.FloorPrefab = floorPrefab;
        cells[coordinates.x, coordinates.y] = newCell;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
        newCell.transform.parent = transform;
        //newCell.transform.localPosition = new Vector3(x - sizeX * 0.5f + 0.5f, 0f, z - sizeZ * 0.5f + 0.5f);
        //newCell.transform.localPosition = new Vector3(coordinates.x * manualFloorScaleX + manualFloorScaleX, 0f, coordinates.y * manualFloorScaleZ + manualFloorScaleZ);
        newCell.transform.localPosition = new Vector3(coordinates.x * floorPrefab.transform.lossyScale.x + floorPrefab.transform.lossyScale.x,
                                                    0f,
                                                    coordinates.y * floorPrefab.transform.lossyScale.z + floorPrefab.transform.lossyScale.z);
        //Debug.Log(newCell.transform.position.ToString());
    }
}