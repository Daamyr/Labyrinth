using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Maze maze;
    public Player player;

	private Maze instanceMaze;
    private Player instancePlayer;

	void Awake(){
	}

	void Start(){
        instanceMaze = Instantiate (maze) as Maze;//cast en Maze sinon il retourne un object
        BeginGame();
	}

	void Update(){
		if (Input.GetKeyDown("r")) { //restart
            RestartGame();
        }
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
