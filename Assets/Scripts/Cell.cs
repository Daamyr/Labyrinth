using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    GameObject m_floorPrefab;
    GameObject m_wallPrefab;

    GameObject m_floor;
    GameObject m_wallN;
    GameObject m_wallE;
    GameObject m_wallS;
    GameObject m_wallW;

    Vector3 m_coordinates;

    Hashtable m_neighbors;

    Maze m_maze;
    Node m_node;

    public Node m_nodePrefab;
    bool m_visited;

    public GameObject debugTo; // dessine un object à chaque case

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

    public Cell(Vector3 _coordinates)
    {
        m_coordinates = _coordinates;
    }

    public List<Cell> NeighborList
    {
        get
        {
            List<Cell> neighbors = new List<Cell>();

            if (m_neighbors["North"] != null)
                neighbors.Add(m_neighbors["North"] as Cell);
            if (m_neighbors["East"] != null)
                neighbors.Add(m_neighbors["East"] as Cell);
            if (m_neighbors["South"] != null)
                neighbors.Add(m_neighbors["South"] as Cell);
            if (m_neighbors["West"] != null)
                neighbors.Add(m_neighbors["West"] as Cell);

            return neighbors;
        }
    }

    public Cell NeighborNorth
    {
        get { return m_neighbors["North"] as Cell; }
    }

    public Cell NeighborEast
    {
        get { return m_neighbors["East"] as Cell; }
    }

    public Cell NeighborSouth
    {
        get { return m_neighbors["South"] as Cell; }
    }

    public Cell NeighborWest
    {
        get { return m_neighbors["West"] as Cell; }
    }
    #endregion

    enum Way
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


    void Start()
    {
        Node newNode = Instantiate(m_nodePrefab, transform.position, new Quaternion());
        newNode.Cell = this;
        newNode.Maze = m_maze;
        newNode.name = "Node " + name;
        m_node = newNode;
        //m_node = new Node(m_maze, this);
    }

    Way whichNeighbor(Cell _cell)
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

    public bool canAccess(Cell _cell)
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

    public void Generate()
    {
        m_floor = Instantiate(m_floorPrefab, m_coordinates, m_floorPrefab.transform.rotation) as GameObject;

        float wallY = m_coordinates.y + m_floorPrefab.transform.localScale.y / 2 + m_wallPrefab.transform.localScale.y / 2;

        Vector3 wallN = new Vector3(m_coordinates.x + m_floor.transform.localScale.x / 2 - m_wallPrefab.transform.localScale.x / 2, wallY, m_coordinates.z);

        Vector3 wallE = new Vector3(m_coordinates.x, wallY, m_coordinates.z - m_floor.transform.localScale.z / 2 + m_wallPrefab.transform.localScale.x / 2);

        Vector3 wallS = new Vector3(m_coordinates.x - m_floor.transform.localScale.x / 2 + m_wallPrefab.transform.localScale.x / 2, wallY, m_coordinates.z);

        Vector3 wallW = new Vector3(m_coordinates.x, wallY, m_coordinates.z + m_floor.transform.localScale.z / 2 - m_wallPrefab.transform.localScale.x / 2);

        m_wallN = Instantiate(m_wallPrefab, wallN, m_floorPrefab.transform.rotation) as GameObject;
        m_wallE = Instantiate(m_wallPrefab, wallE, m_floorPrefab.transform.rotation) as GameObject;
        m_wallS = Instantiate(m_wallPrefab, wallS, m_floorPrefab.transform.rotation) as GameObject;
        m_wallW = Instantiate(m_wallPrefab, wallW, m_floorPrefab.transform.rotation) as GameObject;

        m_wallE.transform.Rotate(new Vector3(0, 90, 0));
        m_wallW.transform.Rotate(new Vector3(0, 90, 0));

        SetNames();
    }
    
    public void PathTo(Cell _cell)
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
            Cell yo = _cells[i].GetComponent<Cell>();
            float distance = Vector3.Distance(transform.position, yo.transform.position);

            //Si la distance == floor.transforme.scale.x/z alors c'est un voisin direct (ligne droite)
            if (distance == m_floor.transform.localScale.x)
            {
                if (yo.transform.position.x == transform.position.x + m_floor.transform.localScale.x && yo.transform.position.z == transform.position.z)
                    m_neighbors.Add("North", yo);

                if (yo.transform.position.x == transform.position.x && yo.transform.position.z == transform.position.z - m_floor.transform.localScale.z)
                    m_neighbors.Add("East", yo);

                if (yo.transform.position.x == transform.position.x - m_floor.transform.localScale.x && yo.transform.position.z == transform.position.z)
                    m_neighbors.Add("South", yo);

                if (yo.transform.position.x == transform.position.x && yo.transform.position.z == transform.position.z + m_floor.transform.localScale.z)
                    m_neighbors.Add("West", yo);
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

    void SetNames()
    {
        m_floor.name = "Floor " + this.name;
        m_wallN.name = "Wall North " + this.name;
        m_wallE.name = "Wall East " + this.name;
        m_wallS.name = "Wall South " + this.name;
        m_wallW.name = "Wall West " + this.name;
    }
}
