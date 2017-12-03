using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public new GameObject gameObject;
    public GameObject camera;

    public float forceSaut = 25f;
    public float maxSaut = 10f;
    private Action forward, back, right, left, jump;

    private GameController gameController;

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

        forward = new Forward();
        back = new Back();
        right = new Right();
        left = new Left();
        jump = new Jump(forceSaut, maxSaut, this.GetComponents<Rigidbody>()[0]);
    }

    Vector3 prevPos;

    // Update is called once per frame
    void FixedUpdate()
    {
        handleKey();
        checkPostion();
    }
    public float range = 1;
    void checkPostion()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, range))
        {
            if (lastPosition == null)
                lastPosition = hit.collider;
            if (hit.collider != lastPosition)
            {
                Cell[,] cells = gameController.Cells;

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

    public void OnCollisionEnter(Collision col)
    {

        //Debug.Log (col.gameObject.name);
        //player.GetComponents<hello> () [0].salut ();

    }

    public void handleKey()
    {
        //public key binding ?
        if (Input.GetKey(KeyCode.W))
        {
            m_state = State.Walking;
            forward.Execute(this, forward);
        }

        if (Input.GetKey(KeyCode.S))
        {
            back.Execute(this, back);
        }

        if (Input.GetKey(KeyCode.D))
        {
            right.Execute(this, right);
        }

        if (Input.GetKey(KeyCode.A))
        {
            left.Execute(this, left);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jump.Execute(this, jump);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameController.MazeInstance.CurrentState !=  Maze.State.CreatingCells)
            {
                PathFollower pathFollower = Instantiate(gameController.pathFollower, transform.position, transform.rotation);

                if(gameController.Path.Count > 0)
                {
                pathFollower.Path = gameController.Path;

                pathFollower.StartCoroutine("FollowPath");
                }
            }
        }
    }
}
