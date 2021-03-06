﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACell : MonoBehaviour
{
    public Node m_nodePrefab;

    protected GameObject m_floorPrefab;
    protected GameObject m_wallPrefab;

    protected GameObject m_floor;
    protected GameObject m_wallN;
    protected GameObject m_wallE;
    protected GameObject m_wallS;
    protected GameObject m_wallW;


    protected Vector3 m_coordinates;

    protected Hashtable m_neighbors;

    protected Maze m_maze;
    protected Node m_node;


    protected bool m_visited;

    protected enum Way
    {
        None,
        North,
        East,
        South,
        West
    }

    void Awake()
    {
        m_visited = false;
    }

    protected Way whichNeighbor(ACell _cell)
    {
        if (_cell == NeighborNorth)
            return Way.North;

        if (_cell == NeighborEast)
            return Way.East;

        if (_cell == NeighborSouth)
            return Way.South;

        if (_cell == NeighborWest)
            return Way.West;

        return Way.None;
    }

    public bool canAccess(ACell _cell)
    {
        Way way = whichNeighbor(_cell);
        if (way == Way.None)
            return false;

        switch (way)
        {
            case Way.North:
                if (m_wallN != null || _cell.m_wallS)
                    return false;
                else
                    return true;

            case Way.East:
                if (m_wallE != null || _cell.m_wallW)
                    return false;
                else
                    return true;

            case Way.South:
                if (m_wallS != null || _cell.m_wallN)
                    return false;
                else
                    return true;

            case Way.West:
                if (m_wallW != null || _cell.m_wallE)
                    return false;
                else
                    return true;

        }

        return false;
    }

    public void PathTo(ACell _cell)
    {
        //suprimer le mur qui sépare la cell de la suivante && supprimer le mur opposé du voisin

        Way way = whichNeighbor(_cell);

        switch (way)
        {
            case Way.North:
                //Debug.Log("--->to the north");
                Destroy(m_wallN);
                Destroy(_cell.m_wallS);
                break;

            case Way.East:
                //Debug.Log("--->to the east");
                Destroy(m_wallE);
                Destroy(_cell.m_wallW);
                break;

            case Way.South:
                //Debug.Log("--->to the south");
                Destroy(m_wallS);
                Destroy(_cell.m_wallN);
                break;

            case Way.West:
                //Debug.Log("--->to the west");
                Destroy(m_wallW);
                Destroy(_cell.m_wallE);
                break;
        }

        //=========================================== Mode debug
        //Vector3 to = _cell.transform.position;
        //to.y += m_wallPrefab.transform.localScale.y / 2;
        //GameObject tmpCube = Instantiate(debugTo, to, new Quaternion());
        //tmpCube.name = "cube";
    }

    public void FindNeighbors()
    {
        GameObject[] _cells = GameObject.FindGameObjectsWithTag("CellShell");
        m_neighbors = new Hashtable();
        for (int i = 0; i < _cells.Length; i++)
        {
            ACell cell = _cells[i].GetComponent<ACell>();
            float distance = Vector3.Distance(transform.position, cell.transform.position);

            //Si la distance == floor.transforme.scale.x/z alors c'est un voisin direct (ligne droite)
            if (distance == m_floor.transform.localScale.x)
            {
                if (cell.transform.position.x == transform.position.x + m_floor.transform.localScale.x && cell.transform.position.z == transform.position.z)
                    m_neighbors.Add("North", cell);

                if (cell.transform.position.x == transform.position.x && cell.transform.position.z == transform.position.z - m_floor.transform.localScale.z)
                    m_neighbors.Add("East", cell);

                if (cell.transform.position.x == transform.position.x - m_floor.transform.localScale.x && cell.transform.position.z == transform.position.z)
                    m_neighbors.Add("South", cell);

                if (cell.transform.position.x == transform.position.x && cell.transform.position.z == transform.position.z + m_floor.transform.localScale.z)
                    m_neighbors.Add("West", cell);
            }
        }

        /*Debug.Log(name + " voisins: ");
        Debug.Log("---> N: " + m_neighbors["North"]);
        Debug.Log("---> E: " + m_neighbors["East"]);
        Debug.Log("---> S: " + m_neighbors["South"]);
        Debug.Log("---> W: " + m_neighbors["West"]);
        Debug.Log(this.NeighborList.Count);
        Debug.Log("----------------------------------------\n");*/

    }

    public void Destroy()
    {
        Destroy(m_node.gameObject);
        Destroy(m_floor.gameObject);
        Destroy(m_wallN.gameObject);
        Destroy(m_wallE.gameObject);
        Destroy(m_wallS.gameObject);
        Destroy(m_wallW.gameObject);
    }

    protected void SetNames()
    {
        m_floor.name = "Floor " + this.name;
        m_wallN.name = "Wall North " + this.name;
        m_wallE.name = "Wall East " + this.name;
        m_wallS.name = "Wall South " + this.name;
        m_wallW.name = "Wall West " + this.name;
    }

    /* =========================================================
     *                  Getters/Setters
     * =========================================================
     */
    #region Getters/Setters
    public bool Visited
    {
        get { return m_visited; }
        set { m_visited = value; }
    }

    public Maze Maze
    {
        set { m_maze = value; }
    }

    public Node Node
    {
        get { return m_node; }
        set { m_node = value; }
    }

    public GameObject FloorPrefab
    {
        get { return m_floorPrefab; }
        set { m_floorPrefab = value; }
    }

    public GameObject WallPrefab
    {
        get { return m_wallPrefab; }
        set { m_wallPrefab = value; }
    }

    public List<GameObject> Walls
    {
        get
        {
            List<GameObject> walls = new List<GameObject>();

            walls.Add(m_wallN);
            walls.Add(m_wallE);
            walls.Add(m_wallS);
            walls.Add(m_wallW);

            return walls;
        }
    }

    public Vector3 Coordinates
    {
        get { return m_coordinates; }
        set { m_coordinates = value; }
    }

    public GameObject Floor
    {
        get { return m_floor; }
    }

    public List<ACell> NeighborList
    {
        get
        {
            List<ACell> neighbors = new List<ACell>();

            if (m_neighbors["North"] != null)
                neighbors.Add(m_neighbors["North"] as ACell);
            if (m_neighbors["East"] != null)
                neighbors.Add(m_neighbors["East"] as ACell);
            if (m_neighbors["South"] != null)
                neighbors.Add(m_neighbors["South"] as ACell);
            if (m_neighbors["West"] != null)
                neighbors.Add(m_neighbors["West"] as ACell);

            return neighbors;
        }
    }

    public StandardCell NeighborNorth
    {
        get { return m_neighbors["North"] as StandardCell; }
    }

    public StandardCell NeighborEast
    {
        get { return m_neighbors["East"] as StandardCell; }
    }

    public StandardCell NeighborSouth
    {
        get { return m_neighbors["South"] as StandardCell; }
    }

    public StandardCell NeighborWest
    {
        get { return m_neighbors["West"] as StandardCell; }
    }
    #endregion
}

