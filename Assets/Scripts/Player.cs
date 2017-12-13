using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public new GameObject gameObject;
    //public GameObject camera;

    public float forceSaut = 25f;
    public float range = 1;

    private AAction forward, back, right, left, plane;
    private Vector3 prevPos;

    private GameController gameController;

    public AAction Forward
    {
        get { return forward; }
    }

    public AAction Back
    {
        get { return back; }
    }

    public AAction Right
    {
        get { return right; }
    }

    public AAction Left
    {
        get { return left; }
    }

    public AAction Plane
    {
        get { return plane; }
    }

    public GameController GameController
    {
        set { gameController = value; }
    }

    public Vector2Int CurrentPosition
    {
        get { return currentPosition; }
    }

    Collider lastPosition;
    Vector2Int currentPosition;

    //public PathFinder aStar;

    IMode m_mode;

    public IMode Mode
    {
        set { m_mode = value; }
    }

    enum State
    {
        Standing,
        Walking
    }

    State m_state;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        m_state = State.Standing;
        //m_mode = new ControlMode();
        forward = new Forward();
        back = new Back();
        right = new Right();
        left = new Left();
        plane = new Plane(forceSaut, this.GetComponents<Rigidbody>()[0]);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        handleKey();
        m_mode.Execute(this);
        checkPostion();
    }

    void checkPostion()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, range))
        {
            if (lastPosition == null)
                lastPosition = hit.collider;
            if (hit.collider != lastPosition)
            {
                ACell[,] cells = gameController.Cells;

                for (int x = 0; x < gameController.MazeSize.x; x++)
                {
                    for (int y = 0; y < gameController.MazeSize.y; y++)
                    {
                        if (cells[x, y].Floor.GetComponent<Collider>() == hit.collider)
                        {
                            lastPosition = hit.collider;
                            currentPosition = new Vector2Int(x, y);
                            gameController.updatePath();
                            return;
                        }
                    }
                }
            }
        }
    }

    public void handleKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))//mode auto
        {
            PathFinder instanceFinder = Instantiate(gameController.finder);
            instanceFinder.name = "Finder from player";
            instanceFinder.Maze = gameController.MazeInstance;

            Vector2Int pos = CurrentPosition;

            instanceFinder.From = gameController.MazeInstance.Cells[pos.x, pos.y];
            instanceFinder.To = gameController.MazeInstance.Cells[gameController.endCell.x, gameController.endCell.y];

            instanceFinder.SearchPath();

            m_mode = new AutoMode();
            List<Vector3> reversePath = instanceFinder.PathList;
            reversePath.Reverse();
            m_mode.m_path = reversePath;
            Debug.Log("AutoMode");

            Destroy(instanceFinder.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))//mode controle
        {
            m_mode = new ControlMode();
            Debug.Log("controlMode");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameController.MazeInstance.CurrentState != Maze.State.CreatingCells && gameController.InstanceFinder.CurrentState != PathFinder.State.Finding)
            {
                PathFollower pathFollower = Instantiate(gameController.pathFollower, transform.position, transform.rotation);

                if (gameController.Path.Count > 0)
                {
                    pathFollower.Path = gameController.Path;

                    pathFollower.StartCoroutine("FollowPath");
                }
            }
        }
    }
}
