using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Maze maze;

	private Maze instance;

	void Awake(){
	}

	void Start(){
		instance = Instantiate (maze) as Maze;//cast en Maze sinon il retourne un object
        BeginGame();
	}

	void Update(){
		if (Input.GetKeyDown("r")) { //restart
            RestartGame();
        }
	}

	private void BeginGame () {
        StartCoroutine(instance.Generate());
    }

	private void RestartGame () {
        if (instance.Finder.CurrentState == PathFinding.State.Finding)
            return;

        instance.CurrentState = Maze.State.CreatingCells;

        StopAllCoroutines();
        instance.KillCells();
		BeginGame();
	}



}
