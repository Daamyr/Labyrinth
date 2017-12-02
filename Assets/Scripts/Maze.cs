using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    enum State
    {
        CreatingCells,
        CreatingPath,
        Finished
    }

    State m_state;

    public Vector2Int beginCell;
    public Vector2Int endCell;
    public Vector2Int mazeSize;


    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public CellTest cellObject;

    public PathFinding aStar;

    private CellTest[,] cells;
    private Vector2Int m_size;

    public Vector2Int Size
    {
        get { return m_size; }
    }

    public CellTest[,] Cells
    {
        get { return cells; }
        set { cells = value; }
    }

    Stack m_path;//chemin final
    Stack m_stack; //stack à dépiller

    void Awake()
    {
        m_size = mazeSize;
    }

    // Use this for initialization
    void Start()
    {
        m_state = State.CreatingCells;
        m_stack = new Stack();
        aStar.Maze = this;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            //Debug.DrawLine(cells[0, 0].transform.position + new Vector3(0, 10, 0), cells[0, 2].transform.position + new Vector3(0, 10, 0), Color.red, 50f,false);
        }
        catch{}

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Maze maze = FindObjectOfType<Maze>();
            Stack path = aStar.SearchPath(maze.Cells[beginCell.x, beginCell.y], maze.Cells[endCell.x, endCell.y]);

            if (path != null)
                Debug.Log("nb stack: " + path.Count);
        }
    }

    public float generationStepDelay;

    public void KillCells()
    {
        for (int i = 0; i < m_size.x; i++)
        {
            for (int y = 0; y < m_size.y; y++)
            {
                //if(cells[i,y] != null)
                //{
                try
                {
                    //Debug.Log(i + " " + y + " " + cells[i, y]);
                    cells[i, y].Destroy();
                    Destroy(cells[i, y]);
                }
                catch (System.Exception) { }
                //}
            }
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("CellShell");
        foreach (GameObject go in gos)
            Destroy(go);
    }

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new CellTest[m_size.x, m_size.y];
        //Debug.Log(cells.Length);
        for (int i = 0; i < m_size.x; i++)
        {
            for (int y = 0; y < m_size.y; y++)
            {
                CreateCell(new Vector2Int(i, y));
                yield return delay;
            }
        }

        m_state = State.CreatingPath; //execute one when all cells are create
        m_stack = new Stack();
        m_stack.Push(cells[beginCell.x, beginCell.y]);
        CreatePath();
    }


    void CreatePath()
    {
        /** JEU DE CONNAISSANCES
         * CellTest begin = cells[2, 2]; possède 4 voisins soit:
         *  -cells[1,2]
         *  -cells[1,2]
         *  -cells[1,2]
         *  -cells[3,2]
         *  
         *  * CellTest begin = cells[0, 1]; possède 3 voisins soit:
         *  -cells[0,0]
         *  -cells[0,2]
         *  -cells[1,1]
         */
        FindAllNeighbors();
        m_state = State.CreatingPath;
        StartCoroutine("hunt");
        //begin.Visited = true;

        //m_stack.Push(begin);

        /*
        do
        {
            CellTest cell = m_stack.Peek() as CellTest;
            Debug.Log("------->peek cell: " + cell.name + " nb dans la stack: " + m_stack.Count);
            cell.Visited = true;
            List<CellTest> possibleCell = new List<CellTest>();
            possibleCell =  cell.NeighborList;
            

            foreach (CellTest neighbor in possibleCell)
            {
                if (neighbor.Visited)
                {
                    possibleCell.Remove(neighbor);
                    break;
                }
            }

            if (possibleCell.Count <= 0)
            {
                m_stack.Pop();
                break;
            }


            int nextCell = Random.Range(0, possibleCell.Count);

            Debug.Log(">next cell: " + possibleCell[nextCell].name);

            cell.PathTo(possibleCell[nextCell]);
            m_stack.Push(possibleCell[nextCell]);
            tries -= 1;
        } while (!AllVisited() && tries > 0);
        */



    }
    public float huntDelay = 1;
    public IEnumerator hunt()
    {
        while (m_state == State.CreatingPath)
        {
            WaitForSeconds delay = new WaitForSeconds(huntDelay);
            CellTest cell = m_stack.Peek() as CellTest;
            //Debug.Log("------->peek cell: " + cell.name + " nb dans la stack: " + m_stack.Count);
            cell.Visited = true;

            if (AllVisited())
            {
                m_state = State.Finished;
                yield break;
            }

            List<CellTest> possibleCell = new List<CellTest>();
            possibleCell = cell.NeighborList;

            bool again;
            do
            {
                again = false;
                foreach (CellTest neighbor in possibleCell)
                {
                    if (neighbor.Visited)
                    {
                        possibleCell.Remove(neighbor);
                        again = true;
                        break;
                    }
                }
            } while (again);

            // Debug.Log("-->nb dans la stack: " + m_stack.Count);
            if (possibleCell.Count <= 0)
            {
                //Debug.Log("-->DEPOP");
                m_stack.Pop();
                //Debug.Log("-->nb dans la stack: " + m_stack.Count);
                //yield break;
            }
            else
            {
                int nextCell = Random.Range(0, possibleCell.Count);
                //Debug.Log(">next cell: " + possibleCell[nextCell].name);


                cell.PathTo(possibleCell[nextCell]);
                m_stack.Push(possibleCell[nextCell]);
            }

            yield return delay;

        }
    }

    bool AllVisited()
    {
        for (int x = 0; x < m_size.x; x++)
        {
            for (int y = 0; y < m_size.y; y++)
            {
                if (cells[x, y].Visited == false)
                {
                    return false;
                }
            }
        }

        Debug.Log("C FINI LOL");
        return true;
    }

    void FindAllNeighbors()
    {
        for (int x = 0; x < m_size.x; x++)
        {
            for (int y = 0; y < m_size.y; y++)
            {
                cells[x, y].FindNeighbors();
            }
        }
    }

    //TODO: Move this code to the a factory
    private void CreateCell(Vector2Int coordinates)
    {
        Vector3 position = new Vector3(coordinates.x * floorPrefab.transform.lossyScale.x + floorPrefab.transform.lossyScale.x,
                                                   0f,
                                                   coordinates.y * floorPrefab.transform.lossyScale.z + floorPrefab.transform.lossyScale.z);

        CellTest newCell = Instantiate(cellObject, position, new Quaternion()) as CellTest;

        newCell.Coordinates = position;
        newCell.FloorPrefab = floorPrefab;
        newCell.WallPrefab = wallPrefab;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
        newCell.Maze = this;

        /*Node newNode = new Node(this, newCell);
        newCell.Node = newNode;*/
        newCell.Generate();


        //Debug.Log("dans createCell() node du newCell: " + newCell.Node);

        //Cell newCell = Instantiate(cellObject) as Cell;
        //newCell.Coordinates = coordinates;

        cells[coordinates.x, coordinates.y] = newCell;
    }
}
