using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFactory : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    GameController m_gameController;

    public GameController GameController
    {
        set { m_gameController = value; }
    }


    // Prefabs for diferents kinds of cell
    public StandardCell standardCellPrefab;
    //BeginCell beingCellPrefab;
    //EndCell endCellPrefab;

    public ACell makeStandardCell(Vector2Int _coordinates)
    {
        Vector3 position = new Vector3(_coordinates.x * floorPrefab.transform.lossyScale.x + floorPrefab.transform.lossyScale.x,
                                       0f,
                                       _coordinates.y * floorPrefab.transform.lossyScale.z + floorPrefab.transform.lossyScale.z);

        StandardCell newCell = Instantiate(standardCellPrefab, position, new Quaternion()) as StandardCell;
        newCell.Coordinates = position;
        newCell.FloorPrefab = floorPrefab;
        newCell.WallPrefab = wallPrefab;
        newCell.name = "Maze StandardCell " + _coordinates.x + ", " + _coordinates.y;
        newCell.Maze = m_gameController.MazeInstance;
        newCell.Generate();

        return newCell;
    }
}
