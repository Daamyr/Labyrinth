using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{
    //G = distance from the last "call"
    //H = distance between the target's path
    float m_G, m_H;
    Node m_parent;
    Maze m_maze;
    StandardCell m_belongsTo;

    #region Getters/Setters
    public Maze Maze
    {
        //get { return m_maze; }
        set { m_maze = value; }
    }

    public float G
    {
        get { return m_G; }
        set { m_G = value; }
    }

    public float H
    {
        get { return m_H; }
        set { m_H = value; }
    }

    public float F
    {
        get { return m_G + m_H; }
    }

    public StandardCell Cell
    {
        get { return m_belongsTo; }
        set { m_belongsTo = value; }
    }

    public Node Parent
    {
        get { return m_parent; }
        set { m_parent = value; }
    }

    public List<Node> Neighbors
    {
        get
        {
            List<Node> list = new List<Node>();

            foreach (var neighborCell in m_belongsTo.NeighborList)
            {
                list.Add(neighborCell.Node);
            }

            return list;
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
        m_G = Mathf.Infinity;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}