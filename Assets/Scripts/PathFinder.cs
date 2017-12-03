using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    List<Node> m_openList, m_closedList;//uniquement les nodes
    Stack m_path;
    Maze m_maze;
    Node m_activeNode;
    State m_state;

    public enum State
    {
        Finding,
        Found,
        NotFound
    }

    #region Getters/Setters
    public State CurrentState
    {
        get { return m_state; }
    }

    public Maze Maze
    {
        set { m_maze = value; }
    }

    public Stack Path
    {
        get { return m_path.Clone() as Stack; }//clone sinon la stack se vide quand on l'utilise ailleurs
    }

    public List<Vector3> PathList //pour la boule
    {
        get
        {
            List<Vector3> vectorPath = new List<Vector3>();
            Stack path = Path;

            while (path.Count > 0)
            {
                Node node = path.Pop() as Node;
                vectorPath.Add(node.Cell.transform.position);
            }

            //Ne pas oublier : si le chemin est en train d'être trouvé, les coordonnées sont à l'envers !
            if (CurrentState != State.Found && vectorPath.Count > 1)
            {
                List<Vector3> reversePath = vectorPath;
                reversePath.Reverse();
                vectorPath = reversePath;
            }

            return vectorPath;
        }
    }

    public Cell From
    {
        get { return m_from; }
        set { m_from = value; }
    }

    public Cell To
    {
        get { return m_to; }
        set { m_to = value; }
    }
    #endregion

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
        m_openList = new List<Node>();
        m_closedList = new List<Node>();

        m_state = State.NotFound;
    }

    // Update is called once per frame
    void Update()
    {

    }


    Stack reconstructPath(Cell _start, Node _solution)
    {
        //Debug.Log("_start: " + _start.name + " | _currentNode: " + _currentNode.name + " | _currentNode.Parent: " + _currentNode.Parent);
        Node node = _solution;
        Stack path = new Stack();

        while (node.Parent != null)
        {
            path.Push(node);
            node = node.Parent;
        }

        m_path = path;
        return path;
    }

    public float delayFinder = 0.5f;

    /**
     * TODO: recréer un object à chaque fois qu'on a un finder à faire OU une fois au debut puis seulement la coroutine ?
     * dans le start, lancer la coroutine
     * calculer le path au fur est à mesure
     * conserver le path à la fin
     * */

    //a redéfinir avant chaque tentative de find
    Cell m_from;
    Cell m_to;

    public IEnumerator FindPath()
    {
        Debug.Log("============= Calcul de l'itineraire ============= ");
        m_openList = null;
        m_closedList = null;
        m_path = null;

        m_path = new Stack();
        m_openList = new List<Node>();
        m_closedList = new List<Node>();

        m_state = State.Finding;

        setAllNodes(m_to);
        m_activeNode = m_from.Node;
        m_activeNode.G = 0;
        m_openList.Add(m_activeNode);


        while (m_openList.Count > 0 && m_state == State.Finding)
        {
            WaitForSeconds delay = new WaitForSeconds(delayFinder);

            Node fastestWay = m_openList[m_openList.Count - 1] as Node;
            for (int i = 0; i < m_openList.Count; i++)
            {
                //verify the solution
                if (m_openList[i] == m_to.Node)
                {
                    m_openList[i].Parent = m_activeNode;
                    m_activeNode = m_openList[i];
                    m_path.Push(m_activeNode);
                    m_state = State.Found;
                    Debug.Log("------->PATH FOUND !!!");
                    reconstructPath(m_from, m_activeNode);
                    drawPath();
                    yield break;
                }
                //else it continues
                else if (m_openList[i].F < fastestWay.F)
                {
                    fastestWay = m_openList[i];
                }
            }

            m_activeNode = fastestWay;

            m_openList.Remove(m_activeNode);
            m_closedList.Add(m_activeNode);

            foreach (var node in m_activeNode.Neighbors)
            {
                float tmp_G = m_activeNode.G + Vector3.Distance(m_activeNode.Cell.transform.position, node.Cell.transform.position);

                if (m_closedList.Contains(node) == false)
                {
                    if (m_activeNode.Cell.canAccess(node.Cell))
                    {
                        if (m_openList.Contains(node) == false)
                        {
                            m_openList.Add(node);
                        }
                        if (tmp_G < node.G)
                        {
                            node.Parent = m_activeNode;
                            node.G = tmp_G;
                            m_path.Push(node);

                            drawPath();
                        }
                    }
                }
            }
            yield return delay;
        }

        if (m_state == State.Finding)
        {
            m_state = State.NotFound;
            Debug.Log("Pas de chemin trouvé entre " + m_from.name + " et " + m_to.name);
        }



    }

    /**
     * Ancienne version:
     * Celle-ci ne se fait pas sur plusieurs frame, donc elle gèle le system pour faire le calcul
    */
    public Stack SearchPath(Cell _from, Cell _to)
    {
        m_path = new Stack();
        m_openList = new List<Node>();
        m_closedList = new List<Node>();
        setAllNodes(_to);
        m_activeNode = _from.Node;
        m_openList.Add(m_activeNode);

        while (m_openList.Count > 0)
        {
            Node fastestWay = m_openList[m_openList.Count - 1] as Node;
            for (int i = 0; i < m_openList.Count; i++)
            {
                //verify the solution
                if (m_openList[i] == _to.Node)
                {
                    m_openList[i].Parent = m_activeNode;
                    m_activeNode = m_openList[i];
                    m_path = reconstructPath(_from, m_activeNode);
                    return m_path;
                }
                //else it continues
                else if (m_openList[i].F < fastestWay.F)
                {
                    fastestWay = m_openList[i];
                }
            }

            m_activeNode = fastestWay;

            m_openList.Remove(m_activeNode);
            m_closedList.Add(m_activeNode);

            foreach (var node in m_activeNode.Neighbors)
            {
                float tmp_G = m_activeNode.G + Vector3.Distance(m_activeNode.Cell.transform.position, node.Cell.transform.position);

                //Debug.Log("node: " + m_activeNode.name + " | neighbor: " + node.name);
                //Debug.Log("m_openList.Contains(node): " + m_openList.Contains(node));
                //Debug.Log("m_activeNode.Cell.canAccess(node.Cell): " + m_activeNode.Cell.canAccess(node.Cell));
                //Debug.Log("tmp_G > node.G: " + (tmp_G > node.G) + " | node.G: " + node.G + " | tmp_G: " + tmp_G);

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

    void drawPath()
    {
        Stack tmp_path = m_path.Clone() as Stack;
        Node tmp_node = tmp_path.Pop() as Node;
        bool draw = true;
        while (draw)
        {
            Debug.DrawLine(tmp_node.Cell.transform.position + new Vector3(0, 5, 0), tmp_node.Parent.transform.position + new Vector3(0, 5, 0), Color.red, 1f, false);
            if (tmp_path.Count > 0)
                tmp_node = tmp_path.Pop() as Node;
            else
                draw = false;
        }
    }

    void setAllNodes(Cell _target)
    {
        for (int x = 0; x < m_maze.Size.x; x++)
        {
            for (int y = 0; y < m_maze.Size.y; y++)
            {
                Cell tmpCell = m_maze.Cells[x, y];
                tmpCell.Node.G = Mathf.Infinity;
                tmpCell.Node.H = Vector3.Distance(tmpCell.transform.position, _target.transform.position);
            }
        }
    }
}
