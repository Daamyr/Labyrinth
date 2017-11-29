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

    public Hashtable Neighbors
    {
        get { return m_neighbors; }
    }

    //List<CellTest> m_neighbors;

    //public List<CellTest> Neighbors
    //{
    //    get { return m_neighbors; }
    //}

    void Start()
    {
        Debug.Log("yo");
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

    void SetNames()
    {
        m_floor.name = "Floor " + this.name;
        m_wallN.name = "Wall North x: " + m_coordinates.x + " y: " + m_coordinates.y;
        m_wallE.name = "Wall East x: " + m_coordinates.x + " y: " + m_coordinates.y;
        m_wallS.name = "Wall South x: " + m_coordinates.x + " y: " + m_coordinates.y;
        m_wallW.name = "Wall West x: " + m_coordinates.x + " y: " + m_coordinates.y;
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
