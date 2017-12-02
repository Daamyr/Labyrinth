using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public enum State
    {
        CreatingCells,
        CreatingPath,
        Finished
    }

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public Cell cellObject;

    public PathFinder aStar;
    PathFinder m_finder;
    State m_state;

    public Vector2Int beginCell;
    public Vector2Int endCell;
    public Vector2Int mazeSize;

    private Cell[,] cells;
    private Vector2Int m_size;
    Stack m_path;//chemin final
    Stack m_stack; //stack à dépiller

    #region Getters/Setters
    public State CurrentState
    {
        get { return m_state; }
        set { m_state = value; }
    }


    public PathFinder Finder
    {
        get { return m_finder; }
    }


    public Vector2Int Size
    {
        get { return m_size; }
    }

    public Cell[,] Cells
    {
        get { return cells; }
        set { cells = value; }
    }
    #endregion


    void Awake()
    {
        m_size = mazeSize;
    }

    // Use this for initialization
    void Start()
    {
        name = "Maze";
        m_state = State.CreatingCells;
        m_stack = new Stack();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (m_state != State.CreatingCells)
                m_finder.StartCoroutine("FindPath");
        }
    }

    public float generationStepDelay;

    public void KillCells()
    {
        for (int i = 0; i < m_size.x; i++)
        {
            for (int y = 0; y < m_size.y; y++)
            {
                try
                {
                    cells[i, y].Destroy();
                }
                catch (System.Exception) { }
            }
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("CellShell");
        foreach (GameObject go in gos)
            Destroy(go);
    }

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new Cell[m_size.x, m_size.y];
        for (int i = 0; i < m_size.x; i++)
        {
            for (int y = 0; y < m_size.y; y++)
            {
                CreateCell(new Vector2Int(i, y));
                yield return delay;
            }
        }

        m_stack = new Stack();
        initFinder();
        m_stack.Push(cells[beginCell.x, beginCell.y]);
        CreatePath();
    }

    void initFinder()
    {
        if (m_finder == null)
            m_finder = Instantiate(aStar);
        m_finder.name = "Finder";
        m_finder.Maze = this;
        m_finder.From = cells[beginCell.x, beginCell.y];
        m_finder.To = cells[endCell.x, endCell.y];
    }

    void CreatePath()
    {
        /** JEU DE CONNAISSANCES
         * Cell begin = cells[2, 2]; possède 4 voisins soit:
         *  -cells[1,2]
         *  -cells[1,2]
         *  -cells[1,2]
         *  -cells[3,2]
         *  
         *  * Cell begin = cells[0, 1]; possède 3 voisins soit:
         *  -cells[0,0]
         *  -cells[0,2]
         *  -cells[1,1]
         */

        FindAllNeighbors();
        m_state = State.CreatingPath;
        StartCoroutine("hunt");

    }
    public float huntDelay = 1;
    public IEnumerator hunt()
    {
        m_state = State.CreatingPath;
        while (m_state == State.CreatingPath)
        {
            WaitForSeconds delay = new WaitForSeconds(huntDelay);
            Cell cell = m_stack.Peek() as Cell;
            cell.Visited = true;

            if (AllVisited())
            {
                m_state = State.Finished;
                yield break;
            }

            List<Cell> possibleCell = new List<Cell>();
            possibleCell = cell.NeighborList;

            bool again;
            do
            {
                again = false;
                foreach (Cell neighbor in possibleCell)
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
                m_stack.Pop();
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

    //TODO: Move this code to the a factory
    private void CreateCell(Vector2Int coordinates)
    {
        Vector3 position = new Vector3(coordinates.x * floorPrefab.transform.lossyScale.x + floorPrefab.transform.lossyScale.x,
                                                   0f,
                                                   coordinates.y * floorPrefab.transform.lossyScale.z + floorPrefab.transform.lossyScale.z);

        Cell newCell = Instantiate(cellObject, position, new Quaternion()) as Cell;

        newCell.Coordinates = position;
        newCell.FloorPrefab = floorPrefab;
        newCell.WallPrefab = wallPrefab;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
        newCell.Maze = this;
        newCell.Generate();

        cells[coordinates.x, coordinates.y] = newCell;
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
}
