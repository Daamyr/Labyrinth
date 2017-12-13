using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCell : ACell
{
    void Awake()
    {
        //m_visited = false;
    }


    void Start()
    {
        Node newNode = Instantiate(m_nodePrefab, transform.position, new Quaternion());
        newNode.Cell = this;
        newNode.Maze = m_maze;
        newNode.name = "Node " + name;
        m_node = newNode;
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
}
