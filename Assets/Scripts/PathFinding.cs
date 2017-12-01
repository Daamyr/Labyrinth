using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    List<Node> m_openList, m_closedList;//uniquement les nodes
    Stack m_path; 
    Maze m_maze;
    Node m_activeNode;

    public Maze Maze
    {
        set { m_maze = value; }
    }

    void Awake()
    {
        //m_maze = maze;
        //m_maze = FindObjectOfType<Maze>().GetComponent<Maze>();
        m_openList = new List<Node>();
        m_closedList = new List<Node>();
        m_path = new Stack();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Stack reconstructPath(CellTest _start, Node _currentNode)
    {
        Node node = _currentNode;
        Stack path = new Stack();

        while(node.Parent != null && node != _start.Node)
        {
            path.Push(node);
            node = node.Parent;
        }

        return path;
    }

    public Stack SearchPath(CellTest _from, CellTest _to)
    {
        setAllH(_to);
        m_activeNode = _from.Node;
        m_openList.Add(m_activeNode);

        while(m_openList.Count > 0)
        {
            Node fastestWay = m_openList[m_openList.Count - 1] as Node;
            foreach (Node node in m_openList)
            {
                if (node.F < fastestWay.F)
                    fastestWay = node;
            }

            m_activeNode = fastestWay;
            
            //verify the solution
            if(m_activeNode.Cell.transform.position == _to.transform.position)
            {
                m_path = reconstructPath(_from, m_activeNode);
                return m_path;
            }
            m_openList.Remove(m_activeNode);
            m_closedList.Add(m_activeNode);

            foreach (var node in m_activeNode.Neighbors)
            {
                if (m_closedList.Contains(node))
                    continue;

                if (m_activeNode.Cell.canAccess(node.Cell))
                {
                    float tmp_G = m_activeNode.G + Vector3.Distance(m_activeNode.Cell.transform.position, node.Cell.transform.position);

                    if (!m_openList.Contains(node))
                        m_openList.Add(node);
                    else if (tmp_G > node.G)
                        continue;

                    node.Parent = m_activeNode;
                    node.G = tmp_G;
                }
            }
        }

        Debug.Log("Pas de chemin trouvé entre " + _from.name + " et " + _to.name);

        return null;
    }

    void setAllH(CellTest _target)
    {
        Debug.Break();
        Debug.Log("maze: " + m_maze + " target: " + _target.Node);
        for(int x = 0; x < m_maze.Size.x; x++)
        {
            for (int y = 0; y < m_maze.Size.y; y++)
            {
                CellTest tmpCell = m_maze.Cells[x, y];
                tmpCell.Node.H = Vector3.Distance(tmpCell.transform.position, _target.transform.position);
            }
        }
    }
}
