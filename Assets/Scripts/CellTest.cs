using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTest : MonoBehaviour{

    GameObject m_floorPrefab;
    GameObject m_wallPrefab;

    bool m_visited = false;

    public bool Visited
    {
        get { return m_visited; }
        set { m_visited = value; }
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

    public List<GameObject> Walls {
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

    GameObject m_floor;
    GameObject m_wallN;
    GameObject m_wallE;
    GameObject m_wallS;
    GameObject m_wallW;

    Vector3 m_coordinates;

    public Vector3 Coordinates
    {
        get { return m_coordinates; }
        set { m_coordinates = value; }
    }

    public CellTest(Vector3 _coordinates)
    {
        m_coordinates = _coordinates;
    }

    Hashtable m_neighbors;
    //Hashtable m_opposit;
    Dictionary<string, string> m_opposit;
    //Dictionary<string, CellTest> m_neighbors;

    enum Ways{
        North,
        East,
        South,
        West
    }

    public List<CellTest> NeighborList
    {
        get
        {
            List<CellTest> neighbors = new List<CellTest>();

            if (m_neighbors["North"] != null)
                neighbors.Add(m_neighbors["North"] as CellTest);
            if (m_neighbors["East"] != null)
                neighbors.Add(m_neighbors["East"] as CellTest);
            if (m_neighbors["South"] != null)
                neighbors.Add(m_neighbors["South"] as CellTest);
            if (m_neighbors["West"] != null)
                neighbors.Add(m_neighbors["West"] as CellTest);

            return neighbors;
        }
    }
    public CellTest NeighborNorth
    {
        get { return m_neighbors["North"] as CellTest; }
    }
    public CellTest NeighborEast
    {
        get { return m_neighbors["East"] as CellTest; }
    }
    public CellTest NeighborSouth
    {
        get { return m_neighbors["South"] as CellTest; }
    }
    public CellTest NeighborWest
    {
        get { return m_neighbors["West"] as CellTest; }
    }

    //List<CellTest> m_neighbors;

    //public List<CellTest> Neighbors
    //{
    //    get { return m_neighbors; }
    //}

    void Start()
    {
        m_opposit = new Dictionary<string, string>();
        /*
        m_opposit["North"] = Ways.South;
        m_opposit["East"] = Ways.West;
        m_opposit["South"] = Ways.North;
        m_opposit["West"] = Ways.East;
        */
    }

    public void Generate()
    {
        m_floor = Instantiate(m_floorPrefab, m_coordinates, m_floorPrefab.transform.rotation) as GameObject;

        float wallY = m_coordinates.y + m_floorPrefab.transform.localScale.y / 2  + m_wallPrefab.transform.localScale.y / 2;

        Vector3 wallN = new Vector3(m_coordinates.x + m_floor.transform.localScale.x /2 - m_wallPrefab.transform.localScale.x / 2, wallY, m_coordinates.z);

        Vector3 wallE = new Vector3(m_coordinates.x, wallY, m_coordinates.z - m_floor.transform.localScale.z / 2 + m_wallPrefab.transform.localScale.x / 2); 

        Vector3 wallS = new Vector3(m_coordinates.x - m_floor.transform.localScale.x /2 + m_wallPrefab.transform.localScale.x / 2, wallY, m_coordinates.z);

        Vector3 wallW = new Vector3(m_coordinates.x, wallY, m_coordinates.z + m_floor.transform.localScale.z / 2 - m_wallPrefab.transform.localScale.x / 2);

        m_wallN = Instantiate(m_wallPrefab, wallN, m_floorPrefab.transform.rotation) as GameObject;
        m_wallE = Instantiate(m_wallPrefab, wallE, m_floorPrefab.transform.rotation) as GameObject;
        m_wallS = Instantiate(m_wallPrefab, wallS, m_floorPrefab.transform.rotation) as GameObject;
        m_wallW = Instantiate(m_wallPrefab, wallW, m_floorPrefab.transform.rotation) as GameObject;

        m_wallE.transform.Rotate(new Vector3(0, 90, 0));
        m_wallW.transform.Rotate(new Vector3(0, 90, 0));

        SetNames();
    }

    public void PathTo(CellTest _cell)
    {
        //suprimer le mur qui sépare la cell de la suivante
        RaycastHit hit;
        print("--->from " + this.name + " to " + _cell.name);
    
        Vector3 from = transform.position;
        from.y += m_wallPrefab.transform.localScale.y / 2;

        Vector3 to = _cell.transform.position;
        to.y += m_wallPrefab.transform.localScale.y / 2;

        if (Physics.Raycast(from, to, out hit))
        {
            //Debug.Log("wall: " + hit.transform.name);
            //détruire le mur quand on le frappe
            /*foreach (var wall in Walls)
            {
                if(wall == hit.transform.gameObject)
                {
                    Destroy(wall);
                }

            }*/
            if (m_wallN == hit.transform.gameObject)
            {
                //Destroy(m_wallN);
                //Destroy(_cell.m_wallS);
            }

            if (m_wallE == hit.transform.gameObject)
            {
                //Destroy(m_wallE);
                //Destroy(_cell.m_wallW);
            }

            if (m_wallS == hit.transform.gameObject)
            {
                //Destroy(m_wallS);
                //Destroy(_cell.m_wallN);
            }

            if (m_wallW == hit.transform.gameObject)
            {
                Destroy(m_wallW);
                Destroy(_cell.m_wallE);
            }

        }

        //appeler une fonction dans cellTest pour supprimer le mur à l'opposé

    }

    void SetNames()
    {
        m_floor.name = "Floor " + this.name;
        m_wallN.name = "Wall North " + this.name;//x: " + m_coordinates.x + " y: " + m_coordinates.y;
        m_wallE.name = "Wall East " + this.name;//x: " + m_coordinates.x + " y: " + m_coordinates.y;
        m_wallS.name = "Wall South " + this.name;//x: " + m_coordinates.x + " y: " + m_coordinates.y;
        m_wallW.name = "Wall West " + this.name;//x: " + m_coordinates.x + " y: " + m_coordinates.y;
    }

    public void FindNeighbors()
    {
        GameObject[] _cells = GameObject.FindGameObjectsWithTag("CellShell");
        //m_neighbors = new List<CellTest>();
        m_neighbors = new Hashtable();
        for (int i = 0; i < _cells.Length; i++)
        {
            CellTest yo = _cells[i].GetComponent<CellTest>();
            float distance = Vector3.Distance(transform.position, yo.transform.position);

            //Si la distance == floor.transforme.scale.x/z alors c'est un voisin direct (ligne droite)
            if(distance == m_floor.transform.localScale.x) {
                //m_neighbors.Add(yo);
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
    }

    public void Destroy()
    {
        //Debug.Log("Je supprime " + name);
        Destroy(m_floor);
        Destroy(m_wallN);
        Destroy(m_wallE);
        Destroy(m_wallS);
        Destroy(m_wallW);
        //Destroy(this);
    }
    
}
