using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    enum State {
        CreatingCells,
        CreatingPath,
        Finished
    }

    State m_state;

    public Vector2Int size;
    public Vector2Int beginPath;

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public CellTest cellObject;

    private CellTest[,] cells;

    public CellTest[,] CellTest
    {
        get { return cells; }
        set { cells = value; }
    }

    Stack m_path;//chemin final
    Stack m_visited; //stack à dépiller

    // Use this for initialization
    void Start()
    {
        m_state = State.CreatingCells;
        m_visited = new Stack();
    }

    // Update is called once per frame
    void Update()
    {

        /* UTILISER UNE COROUTINE
    private int renduX;
    private int renduZ;
        if(renduX < sizeX)
        {
            if(renduZ < sizeZ)
            {
                CreateCell(renduX, renduZ);
                renduZ++;
            }
            else
            {
                renduX++;
                renduZ = 0;
            }
        }
        */

    }

    public float generationStepDelay;

    public void KillCells()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int y = 0; y < size.y; y++)
            {
                //if(cells[i,y] != null)
                //{
                try
                {
                    //Debug.Log(i + " " + y + " " + cells[i, y]);
                    cells[i, y].Destroy();
                    Destroy(cells[i, y]);
                }
                catch (System.Exception){}
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
        cells = new CellTest[size.x, size.y];
        //Debug.Log(cells.Length);
        for (int i = 0; i < size.x; i++)
        {
            for (int y = 0; y < size.y; y++)
            {
                CreateCell(new Vector2Int(i, y));
                yield return delay;
            }
        }

        m_state = State.CreatingPath; //execute one when all cells are create
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
        CellTest begin = cells[beginPath.x, beginPath.y];
        begin.Visited = true;
        
        m_visited.Push(begin);
        
        TODO: recuperer tous les voisins oneShot

        //CellTest cell = m_visited.Peek() as CellTest;
        //List<CellTest> possibleCell = cell.Neighbors;

        //Debug.Log("->cell possible: " + possibleCell.Count);

        foreach (CellTest neighbor in possibleCell)
        {
            //Debug.Log("--->neighbor: " + neighbor.name);
            if (neighbor.Visited)
            {
                possibleCell.Remove(neighbor);
                break;
            }
        }
        //Debug.Log("----->cell possible tri: " + possibleCell.Count);

        //System.Random rand = new System.Random();
        //int nextCell = rand.Next(0, possibleCell.Count);

        int nextCell = Random.Range(0, possibleCell.Count);

        Debug.Log("next cell: " + possibleCell[nextCell].name);

        


    }

    bool AllVisited()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (cells[x, y].Visited)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void FindAllNeighbors()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
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
        newCell.Generate();

        //Cell newCell = Instantiate(cellObject) as Cell;
        //newCell.Coordinates = coordinates;

        cells[coordinates.x, coordinates.y] = newCell;        
    }
}
