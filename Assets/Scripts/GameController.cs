using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Maze maze;
    public Player player;
    public PathFinder finder;
    public Vector2Int endCell;
    public PathFollower pathFollower;
    public CellFactory cellFactory;

    private Maze instanceMaze;
    private Player instancePlayer;
    private PathFinder instanceFinder;
    private CellFactory instanceCellFactory;

    public ACell[,] Cells
    {
        get { return instanceMaze.Cells; }
    }

    public Vector2Int MazeSize
    {
        get { return instanceMaze.Size; }
    }

    public Maze MazeInstance
    {
        get { return instanceMaze; }
    }

    public List<Vector3> Path
    {
        get{ return instanceFinder.PathList; }
    }

    public CellFactory CellFactory
    {
        get { return instanceCellFactory; }
    }

    public PathFinder InstanceFinder
    {
        get { return instanceFinder; }
    }


    void Awake(){
	}

    bool pathHasToBeUpdate = true;

	void Start(){
        instanceMaze = Instantiate (maze) as Maze;//cast en Maze sinon il retourne un object
        instanceMaze.GameController = this;
        instancePlayer = Instantiate(player) as Player;
        instancePlayer.GameController = this;
        instancePlayer.Mode = new ControlMode();
        instancePlayer.transform.position = new Vector3(10,25,10);
        instancePlayer.GetComponent<Rigidbody>().isKinematic = true;
        instanceCellFactory = Instantiate(cellFactory) as CellFactory;
        instanceCellFactory.GameController = this;
        initFinder();
        BeginGame();
	}

	void Update(){
        inputHandler();

        if(pathHasToBeUpdate &&  instanceFinder.CurrentState != PathFinder.State.Finding)
        {
            pathHasToBeUpdate = false;

            //Vector2Int pos = instancePlayer.CurrentPosition;

            //instanceFinder.From = instanceMaze.Cells[pos.x, pos.y];
            //instanceFinder.To = instanceMaze.Cells[endCell.x, endCell.y];

            //instanceFinder.StartCoroutine("FindPath");

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            endCell.x = Random.Range(0, instanceMaze.Size.x);
            endCell.y = Random.Range(0, instanceMaze.Size.y);
        }

    }

    public void freePlayer()
    {
        instancePlayer.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void updatePath()
    {
        //Vector2Int pos = instancePlayer.CurrentPosition;

        pathHasToBeUpdate = true;

        Vector2Int pos = instancePlayer.CurrentPosition;

        instanceFinder.From = instanceMaze.Cells[pos.x, pos.y];
        instanceFinder.To = instanceMaze.Cells[endCell.x, endCell.y];

        instanceFinder.StartCoroutine("FindPath");

        //Debug.Log("Update position joueur x: " + pos.x + " | y:" + pos.y);


        //instanceFinder.From = instanceMaze.Cells[pos.x, pos.y];

        //instanceFinder.From = instanceMaze.Cells[0, 0];
        //instanceFinder.To = instanceMaze.Cells[0, 4];

        //Debug.Log("From: " + instanceMaze.Cells[pos.x, pos.y] + " | to: " + instanceMaze.Cells[endCell.x, endCell.y]);
        //instanceFinder.SearchPath(instanceFinder.From, instanceFinder.To);

        //instanceFinder.StartCoroutine("FindPath");
    }

    void inputHandler()
    {
		if (Input.GetKeyDown("r")) { //restart
            RestartGame();
        }
    }

    void initFinder()
    {
        if (instanceFinder == null)
            instanceFinder = Instantiate(finder);
        instanceFinder.name = "Finder";
        instanceFinder.Maze = instanceMaze;
    }

    private void BeginGame () {
        StartCoroutine(instanceMaze.Generate());
    }

	private void RestartGame () {
        if (instanceMaze.Finder.CurrentState == PathFinder.State.Finding)
            return;

        instanceMaze.CurrentState = Maze.State.CreatingCells;

        StopAllCoroutines();
        instanceMaze.KillCells();
		BeginGame();
	}



}
