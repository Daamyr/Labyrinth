using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    List<Node> m_openList, m_closedList;//uniquement les nodes
    Stack m_path;
    Maze m_maze;
    Node m_activeNode;

    public Maze Maze
    {
        set { m_maze = value; }
    }

    //les variables settée dans awake() restent même après que le programme se soit fermé
    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        m_openList = null;
        m_closedList = null;
        m_path = new Stack();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Stack reconstructPath(CellTest _start, Node _currentNode)
    {
        Debug.Log("_start: " + _start.name + " | _currentNode: " + _currentNode.name + " | _currentNode.Parent: " + _currentNode.Parent);

        Node node = _currentNode;
        Stack path = new Stack();

        while (node.Parent != null && node != _start.Node)
        {
            path.Push(node);
            node = node.Parent;
        }

        return path;
    }

    public Stack SearchPath(CellTest _from, CellTest _to)
    {
        m_openList = new List<Node>();
        m_closedList = new List<Node>();

        setAllH(_to);
        m_activeNode = _from.Node;
        m_openList.Add(m_activeNode);

        while (m_openList.Count > 0)
        {
            Node fastestWay = m_openList[m_openList.Count - 1] as Node;
            foreach (Node node in m_openList)
            {
                if (node.F < fastestWay.F)
                    fastestWay = node;
            }

            m_activeNode = fastestWay;

            //verify the solution
            if (m_activeNode.Cell.transform.position == _to.transform.position)
            {
                m_path = reconstructPath(_from, m_activeNode);
                return m_path;
            }
            m_openList.Remove(m_activeNode);
            m_closedList.Add(m_activeNode);
            Debug.Log("yo");

            foreach (var node in m_activeNode.Neighbors)
            {
                float tmp_G = m_activeNode.G + Vector3.Distance(m_activeNode.Cell.transform.position, node.Cell.transform.position);

                //Debug.Log("node: " + m_activeNode.name + " | neighbor: " + node.name);
                //Debug.Log("m_openList.Contains(node): " + m_openList.Contains(node));
                //Debug.Log("m_activeNode.Cell.canAccess(node.Cell): " + m_activeNode.Cell.canAccess(node.Cell));

                if (m_closedList.Contains(node))
                    continue;

                if (m_activeNode.Cell.canAccess(node.Cell))
                {
                    if (m_openList.Contains(node) == false)
                        m_openList.Add(node);
                    if (tmp_G > node.G)
                        continue;

                    node.Parent = m_activeNode;
                    node.G = tmp_G;
                }
            }

            //Debug.Log("m_activeNode.Neighbors.count: " + m_activeNode.Neighbors.Count);
            //Debug.Log("openstack: " + m_openList.Count);
            //Debug.Log("closedstack: " + m_closedList.Count);
            //Debug.Log("m_activeNode: " + m_activeNode.name);
            //Debug.Break();//stop le programme après une FRAME
        }

        Debug.Log("Pas de chemin trouvé entre " + _from.name + " et " + _to.name);

        return null;
    }

    void setAllH(CellTest _target)
    {
        //Debug.Log("maze: " + m_maze + " target: " + _target.Node);
        for (int x = 0; x < m_maze.Size.x; x++)
        {
            for (int y = 0; y < m_maze.Size.y; y++)
            {
                CellTest tmpCell = m_maze.Cells[x, y];
                tmpCell.Node.H = Vector3.Distance(tmpCell.transform.position, _target.transform.position);
            }
        }
    }
}
