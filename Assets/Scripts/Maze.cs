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

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public CellTest cellObject;

    private CellTest[,] cells;

    public CellTest[,] CellTest
    {
        get { return cells; }
        set { cells = value; }
    }

    // Use this for initialization
    void Start()
    {
        m_state = State.CreatingCells;
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

    Stack path = new Stack();


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
        CellTest begin = cells[3, 3];

        FindNeighbors(begin);


    }

    List<CellTest> FindNeighbors(CellTest cell)
    {
        List<CellTest> neighbors = new List<CellTest>();
        // CellTest[] neighbors = new CellTest[];

        //Collider[] hitColliders = Physics.OverlapSphere(cell.transform.position, cell.FloorPrefab.transform.localScale.x);
        //int i = 0;
        //while (i < hitColliders.Length)
        //{
        //    hitColliders[i].SendMessage("AddDamage");
        //    i++;
        //}

        /*for (int i = 0; i < size.x; i++)
        {
            for (int y = 0; y < size.y; y++)
            {
                //if(cell.)
            }
        }*/
        //GameObject[] _cells = GameObject.FindGameObjectsWithTag("CellShell");

        //for (int i = 0; i < _cells.Length; i++)
        //{
        //    Debug.Log("distance de " + _cells[i].name + ": " + Vector3.Distance(cell.transform.position, _cells[i].transform.position));
        //Si la distance == floor.transforme.scale.x/z alors c'est un voisin direct (ligne droite)
        // }


        cell.FindNeighbors();
        //cell.Neighbors


        for (int i = 0; i < cell.Neighbors.Count; i++)
        {
            Debug.Log("voisin " + (i + 1) + ": " + cell.Neighbors[i].name);
        }

        return neighbors;
    }

    //TODO: Move this code to the a factory
    private void CreateCell(Vector2Int coordinates)
    {
        Vector3 position = new Vector3(coordinates.x * floorPrefab.transform.lossyScale.x + floorPrefab.transform.lossyScale.x,
                                                   0f,
                                                   coordinates.y * floorPrefab.transform.lossyScale.z + floorPrefab.transform.lossyScale.z);
        //CellTest newCell = new CellTest(position);

        //CellTest newCell = new CellTest(position);

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
